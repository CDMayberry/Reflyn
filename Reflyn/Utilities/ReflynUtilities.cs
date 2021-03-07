using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Utilities
{
    public static class ReflynUtilities
    {
        public static NameSyntax GetNameSyntax(string qualifiedName)
        {
            if (string.IsNullOrWhiteSpace(qualifiedName))
            {
                throw new Exception("Empty name syntax passed.");
            }

            string[] splits = qualifiedName.Split('.');

            return GetNameSyntax(splits);
        }

        private static NameSyntax GetNameSyntax(string[] import)
        {
            if (import.Length <= 0)
            {
                throw new Exception("Empty import syntax passed.");
            }

            if (import.Length == 1)
            {
                return IdentifierName(import[0]);
            }

            return QualifiedName(
                GetNameSyntax(import.Take(import.Length - 1).ToArray()),
                IdentifierName(import.Last())
            );
        }

        /// <summary>
        /// Creates a new <see cref="SyntaxTokenList"/> with the specified token added to the end if it is not null.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="token">The token to add.</param>
        public static SyntaxTokenList AddNotNull(this SyntaxTokenList list, SyntaxToken? token)
        {
            if (token == null)
            {
                return list;
            }

            return list.Insert(list.Count, token.Value);
        }
    }
}
