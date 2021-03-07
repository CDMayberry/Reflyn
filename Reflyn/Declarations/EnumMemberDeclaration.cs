using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class EnumMemberDeclaration : MemberDeclaration
    {
        private Expression InitExpression { get; set; } = null;

        public EnumMemberDeclaration(string name, EnumDeclaration declaringType) : base(name, declaringType)
        {
        }

        public EnumMemberDeclaration EqualTo(Expression expr)
        {
            InitExpression = expr;
            return this;
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            EnumMemberDeclarationSyntax enumDec =
                EnumMemberDeclaration(
                    Name
                );

            if (InitExpression != null)
            {
                enumDec = enumDec
                    .WithEqualsValue(
                        EqualsValueClause(
                            InitExpression.ToSyntax()
                        )
                    );
            }

            return enumDec;
        }
    }
}
