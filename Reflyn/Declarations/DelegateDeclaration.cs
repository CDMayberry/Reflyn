using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Mixins;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class DelegateDeclaration : MemberDeclaration, ParametersMixin<DelegateDeclaration>, ScopeMixin<DelegateDeclaration>, StaticMixin<DelegateDeclaration>, MethodAccessMixin<DelegateDeclaration>
    {

        public MethodSignature Signature { get; } = new MethodSignature();
        public SyntaxToken? ScopeModifier { get; set; }
        public SyntaxToken? StaticModifier { get; set; }
        public SyntaxToken? AccessModifier { get; set; }

        internal DelegateDeclaration(string name, Declaration declaringType) : base(name, declaringType)
        {
            this.ToPublic();
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            return DelegateDeclaration(
                    Signature.ReturnType.ToTypeSyntax(),
                    GetIdentifierSyntaxToken()
                )
                .WithDelegateKeyword(
                    Token(SyntaxKind.DelegateKeyword)
                )
                .WithModifiers(
                    GetModifierTokens(ScopeModifier, StaticModifier, AccessModifier)
                )
                .WithAttributeLists(GetCustomAttributes())
                .WithParameterList(
                    Signature.ToSyntax()
                );
        }
    }
}
