using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    // TODO: Not all binary operations are here...how to handle += / -=, etc? Expression base class only has basic math and comparsions, no assignment.
    public class BinaryOpOperatorExpression : Expression
    {
        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public CodeBinaryOperatorTypeReflyn Operator { get; set; }

        public BinaryOpOperatorExpression(Expression left, Expression right, CodeBinaryOperatorTypeReflyn op)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
            this.Operator = op;
        }

        public override ExpressionSyntax ToSyntax()
        {
            return BinaryExpression(
                GetSyntaxKind(),
                // If these are simple variables it'll convert to a IdentifierName(name), if it's a accessor ala `this` it'll do a MemberAccessExpression.
                Left.ToSyntax(),
                Right.ToSyntax()
            );
        }

        // TODO: What do we do with the 
        private SyntaxKind GetSyntaxKind()
        {
            switch (Operator)
            {
                case CodeBinaryOperatorTypeReflyn.Add:
                    return SyntaxKind.AddExpression;
                case CodeBinaryOperatorTypeReflyn.Subtract:
                    return SyntaxKind.SubtractExpression;
                case CodeBinaryOperatorTypeReflyn.Multiply:
                    return SyntaxKind.MultiplyExpression;
                case CodeBinaryOperatorTypeReflyn.Divide:
                    return SyntaxKind.DivideExpression;
                case CodeBinaryOperatorTypeReflyn.Modulus:
                    return SyntaxKind.ModuloExpression;
                case CodeBinaryOperatorTypeReflyn.Assign:
                    break; // Assignment is handled elsewhere if I'm not mistaken.
                case CodeBinaryOperatorTypeReflyn.IdentityInequality: // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-test-for-reference-equality-identity
                    return SyntaxKind.NotEqualsExpression;      // HACK: Not sure about this...
                case CodeBinaryOperatorTypeReflyn.IdentityEquality:
                    return SyntaxKind.EqualsExpression;         // HACK: Not sure about this...
                case CodeBinaryOperatorTypeReflyn.ValueEquality:
                    return SyntaxKind.EqualsExpression;
                case CodeBinaryOperatorTypeReflyn.BitwiseOr:
                    return SyntaxKind.BitwiseOrExpression;
                case CodeBinaryOperatorTypeReflyn.BitwiseAnd:
                    return SyntaxKind.BitwiseAndExpression;
                case CodeBinaryOperatorTypeReflyn.BooleanOr:
                    return SyntaxKind.LogicalOrExpression;
                case CodeBinaryOperatorTypeReflyn.BooleanAnd:
                    return SyntaxKind.LogicalAndExpression;
                case CodeBinaryOperatorTypeReflyn.LessThan:
                    return SyntaxKind.LessThanExpression;
                case CodeBinaryOperatorTypeReflyn.LessThanOrEqual:
                    return SyntaxKind.LessThanOrEqualExpression;
                case CodeBinaryOperatorTypeReflyn.GreaterThan:
                    return SyntaxKind.GreaterThanExpression;
                case CodeBinaryOperatorTypeReflyn.GreaterThanOrEqual:
                    return SyntaxKind.GreaterThanOrEqualExpression;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
