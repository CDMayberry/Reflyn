using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Collections
{
	public class ExpressionList : List<Expression>
    {
        public SyntaxList<ExpressionSyntax> ToSyntaxList()
        {
            return new SyntaxList<ExpressionSyntax>(this.Select(x => x.ToSyntax()));
        }

        public SyntaxNodeOrTokenList ToSyntaxNodeOrTokenList()
        {
            return new SyntaxNodeOrTokenList(this.Select(x => (SyntaxNodeOrToken)x.ToSyntax()));
        }

        public SyntaxNodeOrTokenList ToArgumentSyntaxNodeOrTokenList()
        {
            return new SyntaxNodeOrTokenList(this.Select(x => (SyntaxNodeOrToken)Argument(x.ToSyntax())));
        }
	}
}
