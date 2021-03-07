using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface StaticMixin<T>
    {
        SyntaxToken? StaticModifier { get; set; }
    }

    public static class StaticMixinExtensions
    {
        public static T ToStatic<T>(this StaticMixin<T> mixin)
        {
            mixin.StaticModifier = Token(SyntaxKind.StaticKeyword);
            return (T)mixin;
        }
    }
}