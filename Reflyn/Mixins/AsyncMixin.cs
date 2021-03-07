using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface AsyncMixin<T>
    {
        SyntaxToken? AsyncModifier { get; set; }
    }

    public static class AsyncMixinExtensions
    {
        public static T ToAsync<T>(this AsyncMixin<T> mixin)
        {
            mixin.AsyncModifier = Token(SyntaxKind.AsyncKeyword);
            return (T)mixin;
        }
    }
}
