using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ParenthesisExpression : Expression
    {
        public Expression Expression { get; set; }

        public ParenthesisExpression(Expression expr)
        {
            this.Expression = expr ?? throw new ArgumentNullException(nameof(expr));
        }

        public override ExpressionSyntax ToSyntax()
        {
            return ParenthesizedExpression(
                Expression.ToSyntax()
            );
        }
    }
}
