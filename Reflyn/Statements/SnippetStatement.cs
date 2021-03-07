using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class SnippetStatement : Statement
	{
		private readonly string _statement;

		public SnippetStatement(string statement)
		{
            _statement = statement ?? throw new ArgumentNullException(nameof(statement));
		}

        public override StatementSyntax ToSyntax()
        {
            return ParseStatement(_statement);
        }
	}
}
