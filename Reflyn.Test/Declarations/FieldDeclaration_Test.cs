using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Mixins;
using Reflyn.Test.Dummy;
using Xunit;

namespace Reflyn.Test.Declarations
{
    public class FieldDeclaration_Test
    {
        [Fact]
        public void FieldNameMatches()
        {
            var field = new FieldDeclaration("Test", new DummyDeclaration(), ReflynTestUtils.IntTypeDec);

            field.ToPublic();

            var output = (FieldDeclarationSyntax)field.ToSyntax();

            Assert.True(output.Declaration.Variables.Count > 0, "No Variable Name present.");
            Assert.True(output.Declaration.Variables[0].ToString() == field.Name);
        }

        [Theory]
        [InlineData(typeof(SerializableAttribute))] // Would like to test this with other attributes, also custom attributes
        [InlineData(typeof(DummyCustomAttribute))]
        public void FieldAttributeMatches(Type type)
        {
            var field = new FieldDeclaration("Test", new DummyDeclaration(), ReflynTestUtils.IntTypeDec);

            field.WithCustomAttribute(type);
            var output = (FieldDeclarationSyntax)field.ToSyntax();

            Assert.True(output.AttributeLists.Count > 0, "No Attribute List present.");
            Assert.True(output.AttributeLists[0].Attributes.Count > 0, "No Attribute present.");
            Assert.True(output.AttributeLists[0].Attributes[0].Name.ToString() == type.Name, "Attribute does not match.");
        }

        [Fact]
        public void ToConstAndToStatic_ThrowsExceptionOnToSyntax()
        {
            var field = new FieldDeclaration("Test", new DummyDeclaration(), ReflynTestUtils.IntTypeDec);
            field.ToConst();
            field.ToStatic();
            Assert.Throws<Exception>(() =>
            {
                field.ToSyntax();
            });
        }
    }
}
