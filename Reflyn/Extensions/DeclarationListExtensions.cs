using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Extensions
{

    public static class DeclarationListExtensions
    {
        public static SyntaxNodeOrTokenList ToSeparatedTokenList<T>(this List<T> list) where T : Declaration
        {
            return new SyntaxNodeOrTokenList(list.Select(x => (SyntaxNodeOrToken)x.ToSyntax()).Intersperse(Token(SyntaxKind.CommaToken)));
        }
    }
}
