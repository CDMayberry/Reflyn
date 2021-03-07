using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;

namespace Reflyn.Expressions
{
    // Delegates are low priority
    public class DelegateCreateExpression : Expression
    {
        public ITypeDeclaration DelegateType { get; }

        public MethodReferenceExpression TargetMethod { get; }

        public DelegateCreateExpression(ITypeDeclaration delegateType, MethodReferenceExpression targetMethod)
        {
            this.DelegateType = delegateType ?? throw new ArgumentNullException(nameof(delegateType));
            this.TargetMethod = targetMethod ?? throw new ArgumentNullException(nameof(targetMethod));
        }

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
    }
}
