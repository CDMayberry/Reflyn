using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Expressions;
using Xunit;

namespace Reflyn.Test.Expressions
{
    public class PrimitiveExpression_Test
    {
        [Fact]
        public void IsLiteralExpression()
        {
            PrimitiveExpression<int> prim = Expr.Prim(0);

            var syntax = prim.ToSyntax();

            Assert.True(syntax is LiteralExpressionSyntax);
        }

        [Fact]
        public void HasLiteralToken()
        {
            PrimitiveExpression<int> prim = Expr.Prim(0);

            var syntax = prim.ToSyntax() as LiteralExpressionSyntax;

            var token = syntax.Token;
            var kind = token.Kind();

            Assert.True(kind == SyntaxKind.NumericLiteralToken);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-10)]
        public void IntegerTest(int test)
        {
            PrimitiveExpression<int> intPrim = Expr.Prim(test);

            var syntax = intPrim.ToSyntax() as LiteralExpressionSyntax;
            var token = syntax.Token;

            Assert.True(token.Value is int);
            Assert.Equal(test, (int)token.Value);
        }

        [Theory]
        [InlineData(0.1)]
        [InlineData(1.1)]
        [InlineData(2.1)]
        [InlineData(10.1)]
        [InlineData(-1.1)]
        [InlineData(-2.1)]
        [InlineData(-10.1)]
        public void FloatTest(float test)
        {
            PrimitiveExpression<float> intPrim = Expr.Prim(test);

            var syntax = intPrim.ToSyntax() as LiteralExpressionSyntax;
            var token = syntax.Token;

            Assert.True(token.Value is float);
            Assert.Equal(test, (float)token.Value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BooleanTest(bool test)
        {
            var prim = Expr.Prim(test);

            var syntax = prim.ToSyntax() as LiteralExpressionSyntax;
            var token = syntax.Token;

            Assert.True(token.Value is bool);
            Assert.Equal(test, (bool)token.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello!")]
        public void StringTest(string test)
        {
            var prim = Expr.Prim(test);

            var syntax = prim.ToSyntax() as LiteralExpressionSyntax;
            var token = syntax.Token;

            Assert.True(token.Value is string);
            Assert.Equal(test, (string)token.Value);
        }

        [Theory]
        [InlineData(1)]
        public void ImplicitConversionFromInt(int setTo)
        {
            PrimitiveExpression<int> test = setTo;

            BinaryOpOperatorExpression val = test - 1;

            Assert.Equal(setTo, test.Value);
        }

        [Theory]
        [InlineData(true)]
        public void ImplicitConversionFromBool(bool setTo)
        {
            PrimitiveExpression<bool> test = setTo;

            Assert.Equal(setTo, test.Value);
        }

        /*[Theory]
        [InlineData(1)]
        public void PlusOperatorTest(int setTo)
        {
            PrimitiveExpression<int> test = setTo;

            BinaryOpOperatorExpression val = test - 1;

            Assert.Equal(setTo, test.Value);
        }*/
    }
}
