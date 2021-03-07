using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Utilities
{
    // A copy of the CodeDom version so we can strip CodeDom out.
    public enum FieldDirectionReflyn
    {
        None,
        In,
        Out,
        Ref
    }

    public static class FieldDirectionReflynExtensions
    {
        public static SyntaxToken ToToken(this FieldDirectionReflyn direction)
        {
            switch (direction)
            {
                case FieldDirectionReflyn.In:
                    return Token(SyntaxKind.InKeyword);
                case FieldDirectionReflyn.Out:
                    return Token(SyntaxKind.OutKeyword);
                case FieldDirectionReflyn.Ref:
                    return Token(SyntaxKind.RefKeyword);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
