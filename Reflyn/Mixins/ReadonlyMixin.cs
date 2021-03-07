using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface ReadOnlyMixin<T>
    {
        SyntaxToken? ReadOnlyModifier { get; set; }
    }

    public static class ReadOnlyMixinExtensions
    {
        public static T ToReadOnly<T>(this ReadOnlyMixin<T> mixin)
        {
            mixin.ReadOnlyModifier = Token(SyntaxKind.ReadOnlyKeyword);
            return (T)mixin;
        }

    }


}
