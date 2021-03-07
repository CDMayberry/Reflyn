using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
	public class FieldReferenceExpression : Expression
	{
        public Expression Target { get; }

        public FieldDeclaration DeclaringField { get; }

        public FieldReferenceExpression(Expression target, FieldDeclaration field)
		{
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
			DeclaringField = field ?? throw new ArgumentNullException(nameof(field));
		}

        public override ExpressionSyntax ToSyntax()
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                Target.ToSyntax(), //Target here could be `this`, `base`, a variable, etc.
                IdentifierName(DeclaringField.Name)
            );
        }
	}
}
