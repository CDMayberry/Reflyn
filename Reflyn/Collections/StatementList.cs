using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Expressions;
using Reflyn.Statements;

namespace Reflyn.Collections
{
    public class StatementList : List<Statement>
	{
		public virtual void Add(Expression expression)
        {
            Add(new ExpressionStatement(expression));
        }

		public virtual void AddAssign(Expression left, Expression right)
		{
			Add(Stm.Assign(left, right));
		}

		public void Return(Expression expression)
		{
			Add(Stm.Return(expression));
		}

		public virtual void Insert(int index, Expression expression)
		{
			Insert(index, new ExpressionStatement(expression));
		}

        public SyntaxList<StatementSyntax> ToSyntax()
        {
            return new SyntaxList<StatementSyntax>(this.Select(x => x.ToSyntax()));
		}

        public SyntaxNodeOrTokenList ToSyntaxNodeOrTokenList()
        {
            return new SyntaxNodeOrTokenList(this.Select(x => (SyntaxNodeOrToken)x.ToSyntax()));
		}
	}
}
