using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class ThrowExceptionStatement : Statement
	{
		private readonly Expression _toThrow;

		public ThrowExceptionStatement(Expression toThrow)
		{
            _toThrow = toThrow ?? throw new ArgumentNullException(nameof(toThrow));
		}

        public override StatementSyntax ToSyntax()
        {
            return ThrowStatement(
                _toThrow.ToSyntax()
            );
        }
	}
}
