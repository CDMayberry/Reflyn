using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    // TODO: Expand to multiple types of assignments
	public class AssignStatement : StatementWithExpression
	{
        public Expression Left { get; }

        public Expression Right { get; }

        public AssignStatement(Expression left, Expression right)
		{
            Left = left ?? throw new ArgumentNullException(nameof(left));
			Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override StatementSyntax ToSyntax()
        {
            return ExpressionStatement(
                ToExpressionSyntax()
            );
        }
        public override ExpressionSyntax ToExpressionSyntax()
        {
            return AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    Left.ToSyntax(),
                    Right.ToSyntax()
                )
            ;
        }
	}
}
