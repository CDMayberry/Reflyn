using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class PrimitiveBooleanExpression : PrimitiveExpression<bool>
    {
        public PrimitiveBooleanExpression(bool value) : base(value)
        {
        }

        public override ExpressionSyntax ToSyntax()
        {
            return LiteralExpression(Value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
        }
    }
}
