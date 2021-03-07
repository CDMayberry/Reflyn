using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Statements;

namespace Reflyn.Collections
{
	public class CatchClauseList : List<CatchClause>
	{
        public CatchClauseSyntax[] ToSyntaxArray()
        {
            return this.Select(x => x.ToSyntax()).ToArray();
        }
	}
}
