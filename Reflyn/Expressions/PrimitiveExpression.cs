using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    // HACK: Even though we can check the typeof for enums, a compile-time error would be better...
    // OR: Consider changing this to SimpleExpression, which would include enums. Structs are still a no go however.
    public class PrimitiveExpression<T> : Expression where T : unmanaged
    {
        internal readonly T Value;

        public PrimitiveExpression(T value)
        {
            this.Value = value;
        }

        // TODO: See if we can auto import namespaces so we only need Name instead of FullName.
        public override ExpressionSyntax ToSyntax()
        {
            // Not sure how to do enum values with Refly normally, but this will work for now.
            if (typeof(T).IsEnum)
            {
                return MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(typeof(T).FullName), // This could only be Name if we know the correct namespace is imported...
                    IdentifierName(Value.ToString()) // enum ToString() should return the string representation and not a number.
                );
            }

            return LiteralExpression(
                GetLiteralSyntaxKind(),
                GetLiteralToken()
            );
        }

        private static SyntaxKind GetLiteralSyntaxKind()
        {
            if (IsNumericType())
            {
                return SyntaxKind.NumericLiteralExpression;
            }

            if (typeof(T) == typeof(char))
            {
                return SyntaxKind.CharacterLiteralExpression;
            }

            if (typeof(T) == typeof(string))
            {
                return SyntaxKind.StringLiteralExpression;
            }

            throw new Exception("Not a primitive type.");
        }

        private SyntaxToken GetLiteralToken()
        {

            if (IsNumericType())
            {
                return ToNumericLiteral();
            }

            if (typeof(T) == typeof(char))
            {
                return Literal((char)((object)Value));
            }

            if (typeof(T) == typeof(string))
            {
                return Literal((string)((object)Value));
            }

            throw new Exception("Not a primitive type.");
        }

        private static bool IsNumericType()
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private SyntaxToken ToNumericLiteral()
        {
            // using lowest common denominator.
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                    return Literal((uint)((object)Value));
                case TypeCode.UInt64:
                    return Literal((ulong)((object)Value));
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return Literal((int)((object)Value));
                case TypeCode.Int64:
                    return Literal((long)((object)Value));
                case TypeCode.Single:
                    return Literal((float)((object)Value));
                case TypeCode.Double:
                    return Literal((double)((object)Value));
                case TypeCode.Decimal:
                    return Literal((decimal)((object)Value));

                default:
                    throw new NotImplementedException();
            }
        }

        public static implicit operator PrimitiveExpression<T>(T val) => Expr.Prim(val);
    }
}
