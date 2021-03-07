using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ObjectGenericCreationExpression : ObjectCreationExpression
    {
        private readonly List<ITypeDeclaration> _genericTypes;

        public ObjectGenericCreationExpression(ITypeDeclaration type, ITypeDeclaration[] genericTypes, params Expression[] args) : base(type, args)
        {
            if (genericTypes == null || genericTypes.Length <= 0)
            {
                throw new ArgumentException("No Types passed to FieldGenericDeclaration.", nameof(genericTypes));
            }
            _genericTypes = new List<ITypeDeclaration>(genericTypes);
        }

        public ObjectGenericCreationExpression(ITypeDeclaration type, params Expression[] args) : base(type, args)
        {
            _genericTypes = new List<ITypeDeclaration>();
        }

        public ObjectGenericCreationExpression AddGenericType(string type)
        {
            return AddGenericType(new StringTypeDeclaration(type));
        }

        public ObjectGenericCreationExpression AddGenericType<T>()
        {
            return AddGenericType(typeof(T));
        }

        public ObjectGenericCreationExpression AddGenericType(Type type)
        {
            return AddGenericType(new TypeTypeDeclaration(type));
        }

        public ObjectGenericCreationExpression AddGenericType(ITypeDeclaration type)
        {
            _genericTypes.Add(type);
            return this;
        }

        SyntaxNodeOrToken[] GetGenerics()
        {
            return new SyntaxNodeOrTokenList(_genericTypes.Select(x => (SyntaxNodeOrToken)x.ToTypeSyntax())).Intersperse(Token(SyntaxKind.CommaToken)).ToArray();
        }

        public override ExpressionSyntax ToSyntax()
        {
            if(_genericTypes.Count <= 0)
            {
                throw new Exception("ObjectGenericCreationExpression has no generic types.");
            }

            return ObjectCreationExpression(
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
                .WithArgumentList(
                    Arguments()
                );
        }

    }
}
