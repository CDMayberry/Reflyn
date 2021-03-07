using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Declarations;
using Reflyn.Mixins;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Test.Expressions
{
    public class VisibleDeclaration_Test
    {
        private NamespaceDeclaration namespc;
        private ClassDeclaration testClass;

        public VisibleDeclaration_Test()
        {
            namespc = new NamespaceDeclaration("Dummy");
            testClass = new ClassDeclaration("TestClass", namespc);
        }


        /*[Fact]
        public void HasLiteralToken()
        {
            PrimitiveExpression<int> prim = Expr.Prim(0);

            var syntax = prim.ToSyntax() as LiteralExpressionSyntax;

            var token = syntax.Token;
            var kind = token.Kind();

            Assert.True(kind == SyntaxKind.NumericLiteralToken);
        }*/

        [Fact]
        public void IsPublicByDefault()
        {
            Assert.Equal(Token(SyntaxKind.PublicKeyword), testClass.ScopeModifier);
        }

        [Fact]
        public void ToPrivate()
        {
            testClass.ToPrivate();
            Assert.Equal(Token(SyntaxKind.PrivateKeyword), testClass.ScopeModifier);
        }

        [Fact]
        public void ToPublic()
        {
            testClass.ToPrivate();
            testClass.ToPublic();
            Assert.Equal(Token(SyntaxKind.PublicKeyword), testClass.ScopeModifier);
        }

        [Fact]
        public void AddConstructor()
        {
            testClass.AddConstructor();
        }
    }
}
