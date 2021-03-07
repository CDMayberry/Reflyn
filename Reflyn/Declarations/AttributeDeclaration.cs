using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class AttributeDeclaration//<T> where T : Declaration
    {
        //public T DeclaringType { get; }

        public ITypeDeclaration AttributeType { get; }

        public StringAttributeArgumentDictionary Arguments { get; } = new StringAttributeArgumentDictionary();

        public string Name => AttributeType.Name;

        internal AttributeDeclaration(ITypeDeclaration attributeType)
        {
            this.AttributeType = attributeType ?? throw new ArgumentNullException(nameof(attributeType));
        }
        public virtual string[] GetImports()
        {
            return AttributeType.GetImports().Concat(Arguments.GetImports()).ToArray();
        }
        public AttributeSyntax ToSyntax()
        {
            return Attribute(
                    IdentifierName(Name)
                )
                .WithArgumentList(
                    Arguments.ToSyntax()
                );
        }

        public AttributeDeclaration WithPositional(params Expression[] expressions)
        {
            foreach (var expr in expressions)
            {
                Arguments.AddPositional(expr);
            }

            return this;
        }
        public AttributeDeclaration With(string name, Expression expression)
        {
            Arguments.Add(name, expression);
            return this;
        }
    }
}
