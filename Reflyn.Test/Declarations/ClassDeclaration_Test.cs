using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Declarations;
using Reflyn.Mixins;
using Xunit;

namespace Reflyn.Test.Declarations
{
    public class ClassDeclaration_Test
    {
        private NamespaceDeclaration namespc;
        private ClassDeclaration testClass;
        public ClassDeclaration_Test()
        {
            namespc = new NamespaceDeclaration("Dummy");
            testClass = new ClassDeclaration("TestClass", namespc);
        }

        [Fact]
        public void IsClassByDefault()
        {
            Assert.Equal(ClassOutputType.Class, testClass.OutputType);
        }
        
        [Fact]
        public void ToStruct_SetOutputTypeToStruct()
        {
            testClass.ToStruct();
            Assert.Equal(ClassOutputType.Struct, testClass.OutputType);
        }

        [Fact]
        public void ToInterface_SetOutputTypeToInterface()
        {
            testClass.ToInterface();
            Assert.Equal(ClassOutputType.Interface, testClass.OutputType);
        }

        // NOTE: these are diferent than ScopeMixin tests, these test the *output*, which is separate from the ToPublic calls and is specific to each class.
        [Fact]
        public void ToSyntax_WithToPublic()
        {
            testClass.ToPublic();
            var output = testClass.ToSyntax();
            Assert.Contains(SyntaxKind.PublicKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Fact]
        public void ToSyntax_WithToProtected()
        {
            testClass.ToProtected();
            var output = testClass.ToSyntax();
            Assert.Contains(SyntaxKind.ProtectedKeyword, output.Modifiers.Select(x => x.Kind()));
        }

        [Fact]
        public void ToSyntax_WithToPrivate()
        {
            testClass.ToPrivate();
            var output = testClass.ToSyntax();
            Assert.Contains(SyntaxKind.PrivateKeyword, output.Modifiers.Select(x => x.Kind()));
        }
    }
}
