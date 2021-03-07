using System.Collections.Generic;
using System.Linq;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class StringFieldDeclarationDictionary : Dictionary<string, FieldDeclaration>
	{
        public virtual void Add(FieldDeclaration value)
		{
			base.Add(value.Name, value);
		}

        public string[] GetImports()
        {
            return this.SelectMany(x => x.Value.GetImports()).ToArray();
        }
	}
}
