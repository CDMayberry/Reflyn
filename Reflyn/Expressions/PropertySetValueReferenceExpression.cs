using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class PropertySetValueReferenceExpression : Expression
    {
        public override ExpressionSyntax ToSyntax()
        {
            return IdentifierName("value");
        }
    }
}
