using System;
using System.Collections.Generic;
using Reflyn.Declarations;

namespace Reflyn.Collections
{
	public class DelegateDeclarationList : List<DelegateDeclaration>
	{
        public virtual DelegateDeclaration this[string name]
		{
			get
			{
				foreach (DelegateDeclaration item in this)
				{
					if (item.Name == name)
					{
						return item;
					}
				}
				throw new ApplicationException(name + "() method declaration does not exist.");
			}
		}

		public virtual bool ContainsDelegateName(string name)
		{
			foreach (DelegateDeclaration item in this)
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
