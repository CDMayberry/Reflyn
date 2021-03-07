using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Mixins;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Test.Dummy
{
    public class DummyMemberDeclaration : MemberDeclaration, ScopeMixin<DummyMemberDeclaration>, ClassAccessMixin<DummyMemberDeclaration>
    {
        public DummyMemberDeclaration() : base("Dummy", new DummyDeclaration()) { }

        public DummyMemberDeclaration(string name, DummyDeclaration declaringType) : base(name, declaringType)
        {
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var modifierTokens = GetModifierTokens(ScopeModifier, AccessModifier);
            var attributeList = GetCustomAttributes();

            return FieldDeclaration(
                    VariableDeclaration(
                            PredefinedType(Token(SyntaxKind.IntKeyword))
                        )
                        .WithVariables(
                            SingletonSeparatedList(VariableDeclarator(Name))
                        )
                )
                .WithModifiers(modifierTokens)
                .WithAttributeLists(attributeList);
        }

        public SyntaxToken? ScopeModifier { get; set; }
        public SyntaxToken? AccessModifier { get; set; }
    }
}
