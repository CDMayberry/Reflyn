using System;
using System.Collections.Generic;
using System.Linq;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class MethodDeclarationList : List<MethodDeclaration>
    {
        public virtual MethodDeclaration this[string name]
        {
            get
            {
                foreach (MethodDeclaration item in this)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }
                throw new ApplicationException(name + "() method declaration does not exist.");
            }
        }

        public string[] GetImports()
        {
            return this.SelectMany(x => x.GetImports()).ToArray();
        }

		public virtual bool ContainsMethodName(string name)
		{
			foreach (MethodDeclaration item in this)
			{
				if (item.Name == name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
