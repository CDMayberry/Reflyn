using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Statements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class LambdaExpression : Expression
    {
        private MethodSignature Signature { get; } = new MethodSignature();
        private StatementList Statements { get; } = new StatementList();
        private Expression _firstExpression;

        public LambdaExpression Add(Expression expr)
        {
            // Hacky but we'll see.
            if (_firstExpression == null)
            {
                _firstExpression = expr;
            }

            Statements.Add(expr);
            return this;
        }

        public LambdaExpression Add(Statement stmt)
        {
            Statements.Add(stmt);
            return this;
        }

        public LambdaExpression AddArg(string name)
        {
            Signature.Parameters.AddSimple(name);
            return this;
        }
        public BlockSyntax GetBody()
        {
            return Block(
                Statements.ToSyntax()
            );
        }

        public LambdaExpressionSyntax GetLambda()
        {
            if (Signature.Parameters.Count == 1)
            {
                if (Statements.Count == 1)
                {
                    return SimpleLambdaExpression(
                        Signature.Parameters[0].ToParameterSyntax(),
                        _firstExpression.ToSyntax()
                    );
                }

                return SimpleLambdaExpression(
                    Signature.Parameters[0].ToParameterSyntax(),
                    GetBody()
                );
            }

            if (Statements.Count == 1)
            {
                return ParenthesizedLambdaExpression(
                    Signature.ToSyntax(),
                    _firstExpression.ToSyntax()
                );
            }

            return ParenthesizedLambdaExpression(
                Signature.ToSyntax(),
                GetBody()
            );
        }

        public override ExpressionSyntax ToSyntax()
        {
            return GetLambda();
        }
    }
}
