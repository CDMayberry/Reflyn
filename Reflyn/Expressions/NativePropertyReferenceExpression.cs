using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class NativePropertyReferenceExpression : Expression
    {
        public Expression Target { get; }

        public string Name { get; }

        public NativePropertyReferenceExpression(Expression target, string name)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override ExpressionSyntax ToSyntax()
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                Target.ToSyntax(), //Target here could be `this`, `base`, etc
                IdentifierName(Name)
            );
        }
    }
}
