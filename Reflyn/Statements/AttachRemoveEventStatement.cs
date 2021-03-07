using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;

namespace Reflyn.Statements
{
	// TODO: Convert to Roslyn
	public class AttachRemoveEventStatement : Statement
	{
        public EventReferenceExpression Event { get; }

        public Expression Listener { get; }

        public bool Attach { get; }

        public AttachRemoveEventStatement(EventReferenceExpression eventRef, Expression listener, bool attach)
		{
            Event = eventRef ?? throw new ArgumentNullException(nameof(eventRef));
			Listener = listener ?? throw new ArgumentNullException(nameof(listener));
			Attach = attach;
		}

        public override StatementSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
	}
}
