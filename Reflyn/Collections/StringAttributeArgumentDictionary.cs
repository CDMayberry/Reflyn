using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Expressions;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Collections
{
	public class StringAttributeArgumentDictionary : Dictionary<string, AttributeArgument>
    {
        private int _positional = 0;

        public virtual AttributeArgument AddPositional(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (_named > 0)
            {
                throw new Exception("Cannot add a positional argument after a named argument.");
            }

            var attributeArgument = new AttributeArgument($"__Positional_{_positional++}", expression);
            base.Add(attributeArgument.Name, attributeArgument);
            return attributeArgument;
        }

        private int _named = 0;
		public virtual AttributeArgument Add(string name, Expression expression)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			var attributeArgument = new AttributeArgument(name, expression);
			base.Add(attributeArgument.Name, attributeArgument);
            _named += 1;
			return attributeArgument;
		}

        public string[] GetImports()
        {
            return this.SelectMany(x => x.Value.GetImports()).ToArray();
        }

        public AttributeArgumentListSyntax ToSyntax()
        {
            if (Count <= 0)
            {
				// Null is acceptable by .WithArgumentList(...)
                return null;
            }


            var arguments = 
                this.Select(x => (SyntaxNodeOrToken) x.Value.ToSyntax())
                .Intersperse(Token(SyntaxKind.CommaToken));

            return AttributeArgumentList(
                SeparatedList<AttributeArgumentSyntax>(
					arguments.ToArray()
                )
            );
        }
	}
}
