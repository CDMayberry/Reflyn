using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;

namespace Reflyn.Expressions
{
    // Delegates are low priority
    public class DelegateInvokeExpression : Expression
    {
        public EventReferenceExpression FiredEvent { get; }

        public ExpressionList Parameters { get; } = new ExpressionList();

        public DelegateInvokeExpression(EventReferenceExpression firedEvent, params Expression[] parameters)
        {
            this.FiredEvent = firedEvent ?? throw new ArgumentNullException(nameof(firedEvent));
            this.Parameters.AddRange(parameters);
        }

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
    }
}
