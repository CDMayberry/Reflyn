using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Mixins;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Test.Mixins
{
    public class ScopeMixin_Test
    {
        private ScopeMixin_Dummy dummy;

        public ScopeMixin_Test()
        {
            dummy = new ScopeMixin_Dummy();
        }

        [Fact]
        public void DefaultsToNull()
        {
            Assert.Null(dummy.ScopeModifier);
        }

        [Fact]
        public void ToPublic_SetsScopeModifierToPublic()
        {
            dummy.ToPublic();
            Assert.Equal(Token(SyntaxKind.PublicKeyword), dummy.ScopeModifier);
        }

        [Fact]
        public void ToPrivate_SetsScopeModifierToPrivate()
        {
            dummy.ToPrivate();
            Assert.Equal(Token(SyntaxKind.PrivateKeyword), dummy.ScopeModifier);
        }

        [Fact]
        public void ToProtected_SetsScopeModifierToProtected()
        {
            dummy.ToProtected();
            Assert.Equal(Token(SyntaxKind.ProtectedKeyword), dummy.ScopeModifier);
        }
    }

    public class ScopeMixin_Dummy : ScopeMixin<ScopeMixin_Dummy>
    {
        public SyntaxToken? ScopeModifier { get; set; }
    }
}
