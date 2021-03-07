using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
	public class ConditionStatement : Statement
	{
		private readonly Expression _condition;

        // TODO: need to break this into a series of chainable IfStatement segments
        public StatementList TrueStatements { get; } = new StatementList();

        public StatementList FalseStatements { get; } = new StatementList();

        public ConditionStatement(Expression condition, params Statement[] trueStatements)
		{
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
			TrueStatements.AddRange(trueStatements);
		}

        // TODO: need a better handle on if / else if / else...
        public override StatementSyntax ToSyntax()
        {
            IfStatementSyntax statement = 
            IfStatement(
                _condition.ToSyntax(),
                Block(
                    TrueStatements.ToSyntax()
                )
            );

            if (FalseStatements.Count > 0)
            {
                statement = statement
                .WithElse(
                    ElseClause(
                        Block(
                            FalseStatements.ToSyntax()
                        )
                    )
                );
            }

            return statement;
        }

        public ConditionStatement WithTrue(Statement stmt)
        {
            TrueStatements.Add(stmt);
            return this;
        }

        public ConditionStatement WithTrue(params Statement[] stmts)
        {
            foreach (var stmt in stmts)
            {
                WithTrue(stmt);
            }
            return this;
        }

        public ConditionStatement WithTrue(Expression expression)
        {
            TrueStatements.Add(expression);
            return this;
        }

        public ConditionStatement WithFalse(Statement stmt)
        {
            FalseStatements.Add(stmt);
            return this;
        }

        public ConditionStatement WithFalse(params Statement[] stmts)
        {
            foreach (var stmt in stmts)
            {
                WithFalse(stmt);
            }
            return this;
        }

        public ConditionStatement WithFalse(Expression expression)
        {
            FalseStatements.Add(expression);
            return this;
        }
	}
}
