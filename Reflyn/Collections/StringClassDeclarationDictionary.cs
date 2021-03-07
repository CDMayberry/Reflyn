using System.Collections.Generic;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class StringClassDeclarationDictionary : Dictionary<string, ClassDeclaration>
	{
        public virtual void Add(ClassDeclaration value)
		{
			base.Add(value.Name, value);
		}
	}
}
