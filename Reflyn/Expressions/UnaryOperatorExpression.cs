using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class UnaryOperatorExpression : Expression
    {
        public Expression Target { get; set; }

        public bool Addition { get; }
        public bool PostFix { get; }

        public UnaryOperatorExpression(Expression left, bool addition = true, bool postfix = true)
        {
            this.Target = left ?? throw new ArgumentNullException(nameof(left));
            this.Addition = addition;
            this.PostFix = postfix;

        }

        public override ExpressionSyntax ToSyntax()
        {

            if (PostFix)
            {
                return PostfixUnaryExpression(
                    GetPostKind(),
                    Target.ToSyntax()
                );
            }
            else
            {
                return PrefixUnaryExpression(
                    GetPreKind(),
                    Target.ToSyntax()
                );
            }
        }

        private SyntaxKind GetPostKind()
        {
            return Addition ? SyntaxKind.PostIncrementExpression : SyntaxKind.PostDecrementExpression;
        }

        private SyntaxKind GetPreKind()
        {
            return Addition ? SyntaxKind.PreIncrementExpression : SyntaxKind.PreDecrementExpression;
        }
    }
}
