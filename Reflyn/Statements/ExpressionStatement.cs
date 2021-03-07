using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class ExpressionStatement : Statement
	{
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
		{
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
		}

        public override StatementSyntax ToSyntax()
        {
            return ExpressionStatement(Expression.ToSyntax());
        }
	}
}
