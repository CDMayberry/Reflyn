using System;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class ForEachStatement : Statement
    {
        public ITypeDeclaration LocalType { get; }

        public string LocalName { get; }

        public VariableReferenceExpression Local => new VariableReferenceExpression(new VariableDeclarationStatement(LocalType, LocalName));

        public Expression Collection { get; }

        public StatementList Body { get; } = new StatementList();

        public ForEachType OutputType { get; } = ForEachType.Full;

        public bool EnumeratorDisposable { get; set; }

        public ForEachStatement(ITypeDeclaration localType, string localName, Expression collection, bool enumeratorDisposable)
        {
            LocalType = localType ?? throw new ArgumentNullException(nameof(localType));
            LocalName = localName ?? throw new ArgumentNullException(nameof(localName));
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            EnumeratorDisposable = enumeratorDisposable;
        }

        public override StatementSyntax ToSyntax()
        {
            return ForEachStatement(
                LocalType.ToTypeSyntax(),
                Identifier(LocalName), // this is about the same as doing `Local.ToSyntax()` atm...
                Collection.ToSyntax(), // Collection being accessed.
                Block(
                    Body.ToSyntax()
                )
            );
        }

        public ForEachStatement With(Statement stmt)
        {
            Body.Add(stmt);
            return this;
        }

        public ForEachStatement With(params Statement[] stmts)
        {
            foreach (var stmt in stmts)
            {
                With(stmt);
            }
            return this;
        }

        public StatementList ToCodeDomSnippet()
        {
            throw new Exception("not supported yet");
        }

        public StatementList ToCodeDomFull()
        {
            VariableDeclarationStatement variableDeclarationStatement = Stm.Var(typeof(IEnumerator), "enumerator", Collection.Method("GetEnumerator").Invoke());
            VariableReferenceExpression variableReferenceExpression = Expr.Var(variableDeclarationStatement);
            IterationStatement iterationStatement = Stm.While(variableReferenceExpression.Method("MoveNext").Invoke());
            VariableDeclarationStatement variableDeclarationStatement2 = Stm.Var(LocalType, LocalName);
            variableDeclarationStatement2.InitExpression = variableReferenceExpression.Prop("Current").Cast(LocalType);
            iterationStatement.Statements.Add(variableDeclarationStatement2);
            iterationStatement.Statements.Add(Stm.Comment("foreach body"));
            iterationStatement.Statements.AddRange(Body);

            var sts = new StatementList
            {
                Stm.Comment("<foreach>"),
                Stm.Comment("This loop mimics a foreach call. See C# programming language, pg 247"),
                Stm.Comment("Here, the enumerator is sealed and does not implement IDisposable"),
                variableDeclarationStatement,
                iterationStatement,
                Stm.Comment("</foreach>")
            };
            return sts;
        }
    }
}
