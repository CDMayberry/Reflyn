using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Mixins
{
    public interface AccessMixin<T>
    {
        SyntaxToken? AccessModifier { get; set; }
    }

    public interface AbstractMixin<T> : AccessMixin<T>
    {

    }

    public interface VirtualMixin<T> : AccessMixin<T>
    {

    }

    public interface OverrideMixin<T> : AccessMixin<T>
    {

    }

    public interface SealedMixin<T> : AccessMixin<T>
    {

    }

    // TODO: Move mixed mixins (lol) into a separate file to maintain.
    public interface ClassAccessMixin<T> : AbstractMixin<T>, SealedMixin<T>
    {

    }
    public interface MethodAccessMixin<T> : AbstractMixin<T>, VirtualMixin<T>, OverrideMixin<T>
    {

    }


    public static class AccessMixinExtensions
    {
        public static T ToVirtual<T>(this VirtualMixin<T> mixin)
        {
            mixin.AccessModifier = Token(SyntaxKind.VirtualKeyword);
            return (T)mixin;
        }

        public static T ToOverride<T>(this OverrideMixin<T> mixin)
        {
            mixin.AccessModifier = Token(SyntaxKind.OverrideKeyword);
            return (T)mixin;
        }

        public static T ToAbstract<T>(this AbstractMixin<T> mixin)
        {
            mixin.AccessModifier = Token(SyntaxKind.AbstractKeyword);
            return (T)mixin;
        }

        public static T ToSealed<T>(this SealedMixin<T> mixin)
        {
            mixin.AccessModifier = Token(SyntaxKind.SealedKeyword);
            return (T)mixin;
        }
    }
}
