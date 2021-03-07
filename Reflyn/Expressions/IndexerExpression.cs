using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;

namespace Reflyn.Expressions
{
	public class IndexerExpression : Expression
	{
		private readonly Expression _targetObject;

		private readonly ExpressionList _indices = new ExpressionList();

		public IndexerExpression(Expression targetObject, params Expression[] indices)
		{
            this._targetObject = targetObject ?? throw new ArgumentNullException(nameof(targetObject));
			this._indices.AddRange(indices);
		}

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
	}
}
