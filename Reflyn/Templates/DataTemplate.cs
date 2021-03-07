/*
using Refly.CodeDom;
using System;
using System.Collections;
using System.ComponentModel;

namespace Refly.Templates
{
	public class DataTemplate : Template
	{
		[Category("Data")]
		public string Name;

		[Category("Data")]
		public bool ReadOnly = false;

		[Category("Data")]
		public Hashtable Fields = new Hashtable();

		[Browsable(false)]
		public Hashtable Properties = null;

		public string DataName => string.Format(base.NameFormat, Name);

		public DataTemplate()
			: base("Data", "{0}Data")
		{
		}

		public override void Generate()
		{
			if (Name == null)
			{
				throw new InvalidOperationException("name not set");
			}
			ClassDeclaration classDeclaration = base.NamespaceDeclaration.AddClass(DataName);
			Properties = new Hashtable();
			IDictionaryEnumerator enumerator = Fields.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var dictionaryEntry = (DictionaryEntry)enumerator.Current;
				FieldDeclaration f = classDeclaration.AddField(dictionaryEntry.Value.ToString(), dictionaryEntry.Key.ToString());
				PropertyDeclaration value = classDeclaration.AddProperty(f, hasGet: true, !ReadOnly, checkNonNull: false);
				Properties.Add(dictionaryEntry, value);
			}
			Compile();
		}
	}
}
*/
