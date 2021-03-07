using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class SnippetExpression : Expression
    {
        public string Snippet { get; }

        public SnippetExpression(string snippet)
        {
            this.Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
        }

        public override ExpressionSyntax ToSyntax()
        {
            return ParseExpression(Snippet);
        }
    }
}
