using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Mixins;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
	public class ConstructorDeclaration : Declaration, ScopeMixin<ConstructorDeclaration>, ParametersMixin<ConstructorDeclaration>
	{
		private Declaration _declaringType;

        public MethodSignature Signature { get; } = new MethodSignature();

        public ExpressionList BaseContructorArgs { get; } = new ExpressionList();

        public ExpressionList ChainedContructorArgs { get; } = new ExpressionList();

        public StatementList Body { get; } = new StatementList();

        public ConstructorDeclaration(Declaration declaringType)
			: base("", declaringType.Conformer)
		{
			this._declaringType = declaringType;
            this.ToPublic();
        }
		
		// TODO: Add ToSyntax method
        public override MemberDeclarationSyntax ToSyntax()
        {
            var result = ConstructorDeclaration(
                    Identifier(_declaringType.Name)
                )
                .WithModifiers(
                    GetModifierTokens(ScopeModifier/*, StaticModifier, AccessModifier*/)
                )
                .WithParameterList(
                    Signature.ToSyntax()
                )
                .WithBody(
                    Block(
                        Body.ToSyntax()
                    )
                );

            if (BaseContructorArgs.Count > 0)
            {
                result =
                    result
                        .WithInitializer(
                            ConstructorInitializer(
                                SyntaxKind.BaseConstructorInitializer,
                                ArgumentList(
                                    SeparatedList<ArgumentSyntax>(
                                        BaseContructorArgs.ToArgumentSyntaxNodeOrTokenList().Intersperse(Token(SyntaxKind.CommaToken)).ToArray()
                                    )
                                )
                            )
                        );
            }

            return result;
        }

        public SyntaxToken? ScopeModifier { get; set; }
    }
}
