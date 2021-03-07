using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Mixins;
using Reflyn.Test.Dummy;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Test.Declarations
{
    // TODO: We need to test the Attributes from MemberDeclaration and not in FieldDeclaration... How? Use dummy with a basic setup?

    public class MemberDeclaration_Test
    {
        [Fact]
        public void FieldIsPublic()
        {
            var member = new DummyMemberDeclaration();

            member.ToPublic();

            Assert.True(member.ScopeModifier.HasValue);
            Assert.Equal(Token(SyntaxKind.PublicKeyword), member.ScopeModifier.Value);
            Assert.NotEqual(Token(SyntaxKind.PrivateKeyword), member.ScopeModifier.Value);
        }

        [Fact]
        public void FieldIsPrivate()
        {
            var member = new DummyMemberDeclaration();

            member.ToPrivate();

            Assert.True(member.ScopeModifier.HasValue);
            Assert.Equal(Token(SyntaxKind.PrivateKeyword), member.ScopeModifier.Value);
        }

        [Fact]
        public void FieldIsProtected()
        {
            var member = new DummyMemberDeclaration();

            member.ToProtected();

            Assert.True(member.ScopeModifier.HasValue);
            Assert.Equal(Token(SyntaxKind.ProtectedKeyword), member.ScopeModifier.Value);
        }

        [Fact]
        public void FieldIsAbstract()
        {
            var member = new DummyMemberDeclaration();

            member.ToAbstract();

            Assert.True(member.AccessModifier.HasValue);
            Assert.Equal(Token(SyntaxKind.AbstractKeyword), member.AccessModifier.Value);
        }

        [Fact]
        public void SyntaxIsPublic()
        {
            var field = new DummyMemberDeclaration();

            field.ToPublic();

            var output = field.ToSyntax();
            Assert.True(output.Modifiers.Count > 0, "No Modifiers present.");
            Assert.Contains(SyntaxKind.PublicKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Fact]
        public void SyntaxIsPrivate()
        {
            var field = new DummyMemberDeclaration();

            field.ToPrivate();

            var output = field.ToSyntax();

            Assert.True(output.Modifiers.Count > 0, "No Modifiers present.");
            Assert.Contains(SyntaxKind.PrivateKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Fact]
        public void SyntaxIsProtected()
        {
            var field = new DummyMemberDeclaration();

            field.ToProtected();

            var output = field.ToSyntax();

            Assert.True(output.Modifiers.Count > 0, "No Modifiers present.");
            Assert.Contains(SyntaxKind.ProtectedKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Fact]
        public void SyntaxIsAbstract()
        {
            var field = new DummyMemberDeclaration();

            field.ToAbstract();

            var output = field.ToSyntax();

            Assert.True(output.Modifiers.Count > 0, "No Modifiers present.");
            Assert.Contains(SyntaxKind.AbstractKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Theory]
        [InlineData(typeof(SerializableAttribute))]
        public void HasCustomAttribute(Type type)
        {
            var member = new DummyMemberDeclaration();

            member.WithCustomAttribute(type);

            Assert.Contains(type.FullName, member.CustomAttributes.Select(x => x.AttributeType.FullName));
        }
        

        [Theory]
        [InlineData(typeof(SerializableAttribute))] // Would like to test this with other attributes, also custom attributes
        [InlineData(typeof(DummyCustomAttribute))]
        public void MemberSyntaxAttributeMatches(Type type)
        {
            var member = new DummyMemberDeclaration();

            member.WithCustomAttribute(type);
            MemberDeclarationSyntax output = member.ToSyntax();

            Assert.True(output.AttributeLists.Count > 0, "No Attribute List present.");
            Assert.True(output.AttributeLists[0].Attributes.Count > 0, "No Attribute present.");
            Assert.True(output.AttributeLists[0].Attributes[0].Name.ToString() == type.Name, "Attribute does not match.");
        }

    }
}
