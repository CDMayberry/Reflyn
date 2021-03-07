using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class BaseReferenceExpression : Expression
    {
        public override ExpressionSyntax ToSyntax()
        {
            return BaseExpression();
        }
    }
}
