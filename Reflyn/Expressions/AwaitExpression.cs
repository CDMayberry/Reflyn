using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class AwaitExpression : Expression
    {
        public Expression Expression { get; set; }

        public AwaitExpression(Expression awaitedExpr)
        {
            this.Expression = awaitedExpr ?? throw new ArgumentNullException(nameof(awaitedExpr));
        }

        public override ExpressionSyntax ToSyntax()
        {
            return AwaitExpression(
                Expression.ToSyntax()
            );
        }
    }
}
