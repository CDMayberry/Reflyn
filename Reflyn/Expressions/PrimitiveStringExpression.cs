using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class PrimitiveStringExpression : Expression
    {
        private readonly string _value;

        public PrimitiveStringExpression(string value)
        {
            this._value = value;
        }

        public override ExpressionSyntax ToSyntax()
        {
            return LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                Literal(_value)
            );
        }
    }
}
