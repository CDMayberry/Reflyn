using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;

namespace Reflyn.Expressions
{
    // Delegates and Events are low priority
    public class EventReferenceExpression : Expression
    {
        public Expression Target { get; }

        public EventDeclaration DeclaringEvent { get; }

        public EventReferenceExpression(Expression target, EventDeclaration _event)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            DeclaringEvent = _event ?? throw new ArgumentNullException(nameof(_event));
        }

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
    }
}
