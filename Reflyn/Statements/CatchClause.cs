using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class CatchClause
	{
        public ParameterDeclaration LocalParam { get; }

        public StatementList Statements { get; } = new StatementList();

        public CatchClause(ParameterDeclaration localParam)
		{
            LocalParam = localParam ?? throw new ArgumentNullException(nameof(localParam));
		}

        public CatchClauseSyntax ToSyntax()
        {
            return CatchClause()
                .WithDeclaration(
                    LocalParam.ToCatchSyntax()
                )
                .WithBlock(
                    Block(
                        Statements.ToSyntax()
                    )
                );

        }
	}
}
