using Reflyn.Declarations;
using Xunit;

namespace Reflyn.Test.Declarations
{
    public class EnumDeclaration_Test
    {
        private NamespaceDeclaration namespc;
        private EnumDeclaration testClass;
        public EnumDeclaration_Test()
        {
            namespc = new NamespaceDeclaration("Dummy");
            testClass = new EnumDeclaration("TestClass", namespc, false);
        }

        [Fact]
        public void IsClassByDefault()
        {
            //Assert.Equal(ClassOutputType.Class, testClass.OutputType);
            /*testClass.WithCustomAttribute(typeof(SerializableAttribute));
            testClass.WithCustomAttribute<SerializableAttribute>();
            testClass.WithCustomAttribute<>();
            testClass.WithCustomAttribute<SerializableAttribute>();*/
            //testClass.WithCustomAttribute(typeof(SerializableAttribute));

        }

        [Fact]
        public void SetBaseType()
        {
            testClass.SetBaseType<short>();
            Assert.Equal(testClass.BaseType.FullName, new TypeTypeDeclaration(typeof(short)).FullName);
        }
    }
}
