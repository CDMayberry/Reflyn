using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class FieldArrayDeclaration : FieldDeclaration
    {
        internal FieldArrayDeclaration(string name, Declaration declaringType, ITypeDeclaration type) : base(name, declaringType, type)
        {
        }
        public new FieldArrayDeclaration EqualTo(Expression expr)
        {
            InitExpression = expr;
            return this;
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var modifierTokens = GetModifierTokens();
            var attributeList = GetCustomAttributes();

            return FieldDeclaration(
                    VariableDeclaration(
                            Type.ToArrayTypeSyntax()
                        )
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(Name)
                                    .WithInitializer(InitExpression != null ? EqualsValueClause(InitExpression.ToSyntax()) : null)
                            )
                        )
                )
                .WithModifiers(modifierTokens)
                .WithAttributeLists(attributeList);
        }

        // Unused, InitExpression should be using Expr.NewArray (or whatever it's called)
        private ArrayCreationExpressionSyntax ArrayInit()
        {
            return ArrayCreationExpression(
                Type.ToArrayTypeSyntax()
                    .WithRankSpecifiers(
                        SingletonList(
                            ArrayRankSpecifier(
                                SingletonSeparatedList(
                                    InitExpression.ToSyntax()
                                )
                            )
                        )
                    )
            );
        }

        public Expression Length => ToExpr().Prop("Length");

        public ArrayIndexerExpression ArrayItem(int index)
        {
            return ToExpr().ArrayItem(index);

        }

        public ArrayIndexerExpression ArrayItem(Expression index)
        {
            return ToExpr().ArrayItem(index);
        }
    }
}
