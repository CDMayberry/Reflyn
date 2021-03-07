using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ArrayCreationWithSizeExpression : Expression
    {
        private readonly ITypeDeclaration type;

        // CONSIDER: Test if size is a LiteralExpression with a SyntaxKind.NumericLiteralExpression.
        public Expression SizeExpression { get; }

        public ArrayCreationWithSizeExpression(ITypeDeclaration type, Expression sizeExpression)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.SizeExpression = sizeExpression ?? throw new ArgumentNullException(nameof(sizeExpression));
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
                                SingletonSeparatedList(
                                    SizeExpression.ToSyntax()
                                )
                            )
                        )
                    )
            );
        }
    }
}
