using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class PrimitiveNullExpression : Expression
    {
        public override ExpressionSyntax ToSyntax()
        {
            return LiteralExpression(
                SyntaxKind.NullLiteralExpression
            );
        }
    }
}
