using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;

namespace Reflyn.Expressions
{
    // TODO: Should this just become CodeSnippets? I don't know if Roslyn has the concept of Native Expressions...
    public class NativeMethodInvokeExpression : Expression
    {
        public NativeMethodReferenceExpression InvokedMethod { get; }

        public ExpressionList Parameters { get; } = new ExpressionList();

        public NativeMethodInvokeExpression(NativeMethodReferenceExpression method, params Expression[] parameters)
        {
            InvokedMethod = method ?? throw new ArgumentNullException(nameof(method));
            this.Parameters.AddRange(parameters);
        }

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
    }
}
