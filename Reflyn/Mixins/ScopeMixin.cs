using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface ScopeMixin<T>
    {
        SyntaxToken? ScopeModifier { get; set; }
    }

    public static class ScopeMixinExtensions
    {
        public static T ToPublic<T>(this ScopeMixin<T> mixin)
        {
            mixin.ScopeModifier = Token(SyntaxKind.PublicKeyword);
            return (T)mixin;
        }

        public static T ToPrivate<T>(this ScopeMixin<T> mixin)
        {
            mixin.ScopeModifier = Token(SyntaxKind.PrivateKeyword);
            return (T)mixin;
        }

        public static T ToProtected<T>(this ScopeMixin<T> mixin)
        {
            mixin.ScopeModifier = Token(SyntaxKind.ProtectedKeyword);
            return (T)mixin;
        }
    }
}
