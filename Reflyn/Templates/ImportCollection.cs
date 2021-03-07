using System.Collections;

namespace Reflyn.Refly.Templates
{
	public class ImportCollection : ArrayList
	{
		public new Import this[int index]
		{
			get
			{
				return (Import)base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		public void Add(string import)
		{
			Add(new Import(import));
		}

		public void Add(Import import)
		{
			base.Add(import);
		}
	}
}
