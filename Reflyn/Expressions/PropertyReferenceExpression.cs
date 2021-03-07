using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class PropertyReferenceExpression : Expression
    {
        public Expression Target { get; }

        // TODO: Convert to take a string, we don't particularly care about anything except the Name.
        public PropertyDeclaration Property { get; }

        // TODO: Add a string variant and remove NativePropertyReferenceExpression.
        public PropertyReferenceExpression(Expression target, PropertyDeclaration property)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public override ExpressionSyntax ToSyntax()
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                Target.ToSyntax(), //Target here could be `this`, `base`, etc
                IdentifierName(Property.Name)
            );
        }
    }
}
