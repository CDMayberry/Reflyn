using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class TryCatchFinallyStatement : Statement
	{
        public StatementList Try { get; } = new StatementList();

        public CatchClauseList CatchClauses { get; } = new CatchClauseList();

        public StatementList Finally { get; } = new StatementList();

        public override StatementSyntax ToSyntax()
        {
            var statement = TryStatement(
                    List(
                        CatchClauses.ToSyntaxArray()
                    )
                )
                .WithBlock(
                    Block(
                        Try.ToSyntax()
                    )
                );

            if(Finally.Count > 0)
            {
                statement = statement.WithFinally(
                    FinallyClause(
                        Block(
                            Finally.ToSyntax()
                            )
                        )
                );
            }

            return statement;
        }
	}
}
