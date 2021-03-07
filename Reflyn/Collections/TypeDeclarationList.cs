using System;
using System.Collections.Generic;
using System.Linq;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class TypeDeclarationList : List<ITypeDeclaration>
	{
        public virtual void Add(Type value)
		{
			base.Add(new TypeTypeDeclaration(value));
		}

		public string[] GetImports()
        {
            return this.SelectMany(x => x.GetImports()).ToArray();
        }
	}
}
