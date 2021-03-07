using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reflyn.Statements
{
	public class NativeStatement : Statement
	{
		/*private readonly CodeStatement _statement;

		public NativeStatement(CodeStatement statement)
		{
            _statement = statement ?? throw new ArgumentNullException(nameof(statement));
		}*/

        public override StatementSyntax ToSyntax()
        {
            //ParseStatement(_statement.ToString());
            throw new NotImplementedException();
        }
	}
}
