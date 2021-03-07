using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class FieldGenericDeclaration : FieldDeclaration
    {
        private readonly List<ITypeDeclaration> _genericTypes;

        internal FieldGenericDeclaration(string name,Declaration declaringType, ITypeDeclaration type, params ITypeDeclaration[] genericTypes) : base(name, declaringType, type)
        {
            _genericTypes = new List<ITypeDeclaration>(genericTypes);
        }

        // TODO: We need a EqualToNew that can read it's own type so we don't have to copy types (and potentially miss one if it changes)
        public new FieldGenericDeclaration EqualTo(Expression expr)
        {
            InitExpression = expr;
            return this;
        }

        // TODO: need to add a params Expression parameters
        public FieldGenericDeclaration EqualToNew(params Expression[] parameters)
        {
            ObjectGenericCreationExpression expr = Expr.NewGeneric(Type, _genericTypes.ToArray(), parameters);

            InitExpression = expr;
            return this;
        }

        public FieldGenericDeclaration AddGenericType(string type)
        {
            return AddGenericType(new StringTypeDeclaration(type));
        }

        public FieldGenericDeclaration AddGenericType<T>()
        {
            return AddGenericType(typeof(T));
        }

        public FieldGenericDeclaration AddGenericType(Type type)
        {
            return AddGenericType(new TypeTypeDeclaration(type));
        }

        public FieldGenericDeclaration AddGenericType(ITypeDeclaration type)
        {
            _genericTypes.Add(type);
            return this;
        }

        SyntaxNodeOrToken[] GetGenerics()
        {
            return new SyntaxNodeOrTokenList(_genericTypes.Select(x => (SyntaxNodeOrToken)x.ToTypeSyntax())).Intersperse(Token(SyntaxKind.CommaToken)).ToArray();
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            if (_genericTypes == null || _genericTypes.Count <= 0)
            {
                throw new ArgumentException("No Types passed to FieldGenericDeclaration.", nameof(_genericTypes));
            }

            var modifierTokens = GetModifierTokens();
            var attributeList = GetCustomAttributes();

            return FieldDeclaration(
                    VariableDeclaration(
                        GenericName(
                                Identifier(Type.Name)
                            )
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        GetGenerics()
                                    )
                                )
                            )
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
    }
}
