using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ArrayCreationWithInitializersExpression : Expression
    {
        private readonly ITypeDeclaration type;

        private readonly ExpressionList initializers = new ExpressionList();

        public ArrayCreationWithInitializersExpression(ITypeDeclaration type, params Expression[] initializers)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.initializers.AddRange(initializers);
        }

        public override ExpressionSyntax ToSyntax()
        {
            return ArrayCreationExpression(
                ArrayType(
                        type.ToTypeSyntax()
                    )
                    .WithRankSpecifiers(
                        SingletonList(
                            ArrayRankSpecifier(
                                SingletonSeparatedList<ExpressionSyntax>(
                                    OmittedArraySizeExpression()
                                )
                            )
                        )
                    )
            )
                .WithInitializer(
                    InitializerExpression(
                        SyntaxKind.ArrayInitializerExpression,
                        SeparatedList<ExpressionSyntax>(
                            // CONSIDER: Initializers should be either a reference to variables or a literal expression. Consider adding a check to verify.
                            initializers.ToSyntaxNodeOrTokenList().Intersperse(Token(SyntaxKind.CommaToken)).ToArray()
                        )
                    )
                );
        }
    }
}
