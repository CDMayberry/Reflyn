using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public interface ITypeDeclaration
    {
        string FullName { get; }

        string Name { get; }

        string[] GetImports();
    }

    public static class ITypeDeclarationExtension {
        public static TypeSyntax ToTypeSyntax(this ITypeDeclaration itype)
        {
            // nameof(Void) not possible.
            if (itype.Name == "Void")
            {
                return PredefinedType(
                    Token(SyntaxKind.VoidKeyword)
                );
            }

            if (itype.Name == nameof(Boolean))
            {
                return PredefinedType(
                    Token(SyntaxKind.BoolKeyword)
                );
            }

            if (itype.Name == nameof(Byte))
            {
                return PredefinedType(
                    Token(SyntaxKind.ByteKeyword)
                );
            }

            if (itype.Name == nameof(SByte))
            {
                return PredefinedType(
                    Token(SyntaxKind.SByteKeyword)
                );
            }

            if (itype.Name == nameof(Int16))
            {
                return PredefinedType(
                    Token(SyntaxKind.ShortKeyword)
                );
            }

            if (itype.Name == nameof(UInt16))
            {
                return PredefinedType(
                    Token(SyntaxKind.UShortKeyword)
                );
            }

            if (itype.Name == nameof(Int32))
            {
                return PredefinedType(
                    Token(SyntaxKind.IntKeyword)
                );
            }

            if (itype.Name == nameof(UInt32))
            {
                return PredefinedType(
                    Token(SyntaxKind.UIntKeyword)
                );
            }

            if (itype.Name == nameof(Single))
            {
                return PredefinedType(
                    Token(SyntaxKind.FloatKeyword)
                );
            }

            if (itype.Name == nameof(Double))
            {
                return PredefinedType(
                    Token(SyntaxKind.DoubleKeyword)
                );
            }

            if (itype.Name == nameof(String))
            {
                return PredefinedType(
                    Token(SyntaxKind.StringKeyword)
                );
            }

            // With proper usage of GetImports we only need to use .Name, though with StringTypeDeclarations this could be different...
            return IdentifierName(itype.Name);
        }
        public static ArrayTypeSyntax ToArrayTypeSyntax(this ITypeDeclaration itype)
        {
            if (itype.Name == "Void")
            {
                throw new Exception("Void cannot be a array type.");
            }

            // With proper usage of GetImports we only need to use .Name, though with StringTypeDeclarations this could be different...
            // Not sure about OmittedArraySize for non-fields or properties, but lets let it be for now.
            return ArrayType(IdentifierName(itype.Name), SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));
        }


    }
}
