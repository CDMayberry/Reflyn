using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface ConstMixin<T>
    {
        SyntaxToken? ConstModifier { get; set; }
    }


    public static class ConstMixinExtensions
    {
        public static T ToConst<T>(this ConstMixin<T> mixin)
        {
            mixin.ConstModifier = Token(SyntaxKind.ConstKeyword);
            return (T)mixin;
        }
    }
}
