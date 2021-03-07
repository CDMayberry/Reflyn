using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class IterationStatement : Statement
	{
		private readonly VariableDeclarationStatement _initStatement;

		private readonly StatementWithExpression _incrementStatement;

        // TODO: Do we have some way to enforce this returns a BinaryExpression?
		private readonly Expression _testExpression;

        public StatementList Statements { get; } = new StatementList();

        public IterationStatement(VariableDeclarationStatement initStatement, Expression testExpression, StatementWithExpression incrementStatement)
		{
            _initStatement = initStatement;
			_testExpression = testExpression ?? throw new ArgumentNullException(nameof(testExpression));
			_incrementStatement = incrementStatement;
        }

        // This should be broken out to a different While Statement
        public IterationStatement(Expression testExpression)
        {
            _initStatement = null;
            _testExpression = testExpression ?? throw new ArgumentNullException(nameof(testExpression));
            _incrementStatement = null;
        }
        public IterationStatement With(Expression expr)
        {
            return With(Stm.FromExpr(expr));
        }

        public IterationStatement With(Statement stmt)
        {
            Statements.Add(stmt);
            return this;
        }

        public IterationStatement With(params Statement[] stmts)
        {
            foreach (var stmt in stmts)
            {
                With(stmt);
            }
            return this;
        }

        private StatementSyntax ToWhileSyntax()
        {
            return WhileStatement(
                _testExpression.ToSyntax(),
                Block(
                    Statements.ToSyntax()
                )
            );
        }

		private StatementSyntax ToForSyntax()
		{
            return ForStatement(
                    Block(
                        Statements.ToSyntax()
                    )
                )
                .WithDeclaration(
                    _initStatement.GetDeclaration()
                )
                .WithCondition(
                    _testExpression.ToSyntax()
                )
                .WithIncrementors(
                    SingletonSeparatedList(
                    _incrementStatement.ToExpressionSyntax()
                    )
                );
        }

        public override StatementSyntax ToSyntax()
        {
            if (_initStatement == null && _incrementStatement == null)
            {
                return ToWhileSyntax();
            }
            else
            {
                return ToForSyntax();
            }
        }
	}
}
