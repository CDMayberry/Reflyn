using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class UsingStatement : Statement
    {
        private readonly string _name;
        private readonly Expression _assignment;
        public StatementList Statements { get; } = new StatementList();

        public UsingStatement(string name, Expression assignTo, params Statement[] statements)
        {
            _name = name;
            _assignment = assignTo ?? throw new ArgumentNullException(nameof(assignTo));
            Statements.AddRange(statements);
        }

        public UsingStatement With(Statement stmt)
        {
            Statements.Add(stmt);
            return this;
        }

        public UsingStatement With(params Statement[] stmts)
        {
            foreach (var stmt in stmts)
            {
                With(stmt);
            }
            return this;
        }

        public override StatementSyntax ToSyntax()
        {
            return UsingStatement(
                    Block(
                        Statements.ToSyntax()
                    )
                )
                .WithDeclaration(
                    VariableDeclaration(
                            IdentifierName("var")
                        )
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                        Identifier(_name)
                                    )
                                    .WithInitializer(
                                        EqualsValueClause(
                                            _assignment.ToSyntax()
                                        )
                                    )
                            )
                        )
                );
        }

        public VariableReferenceExpression ToExpr()
        {
            return Expr.Var(_name);
        }
    }
}
