using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Mixins;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class EventDeclaration : MemberDeclaration, ScopeMixin<EventDeclaration>
    {
        public ITypeDeclaration Type { get; }

        internal EventDeclaration(string name, Declaration declaringType, ITypeDeclaration type)
            : base(name, declaringType)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.ToPublic();
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            return EventFieldDeclaration(
                    VariableDeclaration(
                            Type.ToTypeSyntax()
                        )
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                    Identifier(Name)
                                )
                            )
                        )
                )
                .WithAttributeLists(GetCustomAttributes())
                .WithModifiers(
                    GetModifierTokens(ScopeModifier)
                );
        }

        public SyntaxToken? ScopeModifier { get; set; }
    }
}
