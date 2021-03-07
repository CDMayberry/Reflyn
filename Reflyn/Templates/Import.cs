using System.ComponentModel;

namespace Reflyn.Refly.Templates
{
	public class Import
	{
        [Category("Data")]
		public string Name { get; set; } = "";

        public Import()
		{
		}

		public Import(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return $"{Name}";
		}
	}
}
