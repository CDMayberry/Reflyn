using System.Collections.Generic;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class StringEnumDeclarationDictionary : Dictionary<string, EnumDeclaration>
	{
        public virtual void Add(EnumDeclaration value)
		{
			base.Add(value.Name, value);
		}
	}
}
