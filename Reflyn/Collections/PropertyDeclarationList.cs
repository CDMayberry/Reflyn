using System.Collections.Generic;
using System.Linq;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
    //[Obsolete("Conversion to generics has made this obsolete, use a List<Type> instead.")]
	public class PropertyDeclarationList : List<PropertyDeclaration>
	{
        public string[] GetImports()
        {
            return this.SelectMany(x => x.GetImports()).ToArray();
        }
	}
}
