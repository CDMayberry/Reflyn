using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Collections
{
	public class ParameterDeclarationList : List<ParameterDeclaration>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nonNull"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual ParameterDeclaration AddSimple(string name, bool nonNull = true, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            var parameterDeclaration = new ParameterDeclaration(name, nonNull, direction);
            base.Add(parameterDeclaration);
            return parameterDeclaration;
        }

		public virtual ParameterDeclaration Add(ITypeDeclaration type, string name, bool nonNull = true, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
		{
			var parameterDeclaration = new ParameterDeclaration(type, name, nonNull, direction);
			base.Add(parameterDeclaration);
			return parameterDeclaration;
		}

		public virtual ParameterDeclaration Add(Type type, string name, bool nonNull = true, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
		{
			return Add(new TypeTypeDeclaration(type), name, nonNull, direction);
		}

		public virtual ParameterDeclaration Add(string type, string name, bool nonNull = true, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
		{
			return Add(new StringTypeDeclaration(type), name, nonNull, direction);
		}

        public string[] GetImports()
        {
            return this.SelectMany(x => x.GetImports()).ToArray();
        }

		/// <summary>
		/// Converts to Parameter wrapped Syntaxs with CommaTokens interspersed.
		/// </summary>
		/// <returns></returns>
        public SyntaxNodeOrToken[] ToSyntaxArray()
        {
            return this.Select(x => x.ToSyntax()).Intersperse(Token(SyntaxKind.CommaToken)).ToArray();
		}
	}
}
