using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using Reflyn.Collections;

namespace Reflyn.Documentation
{
	public static class TypeFinder
	{
		private static readonly AssemblyList Assemblies;

		private static readonly StringDictionary Names;

		private static readonly Regex Word;

		private static readonly StringCollection Imports;

        static TypeFinder()
		{
			Assemblies = new AssemblyList();
			Names = new StringDictionary();
			Word = new Regex("\\w", RegexOptions.Multiline | RegexOptions.Compiled);
			Imports = new StringCollection();
			AddAssembly(typeof(int).Assembly);
		}

		public static void AddImports(string ns)
		{
			lock (typeof(TypeFinder))
			{
				Imports.Add(ns);
			}
		}

		public static void AddAssembly(Assembly assembly)
		{
			lock (typeof(TypeFinder))
			{
				Assemblies.Add(assembly);
				Type[] exportedTypes = assembly.GetExportedTypes();
				foreach (Type type in exportedTypes)
				{
					Names.Add(type.FullName, null);
				}
			}
		}

		public static string CrossRef(string text)
		{
			return Word.Replace(text, MatchEval);
		}

		private static string GetType(string name)
		{
			if (Names.ContainsKey(name))
			{
				return name;
			}
			StringEnumerator enumerator = Imports.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = $"{current}.{name}";
				if (Names.ContainsKey(text))
				{
					return text;
				}
			}
			return null;
		}

		private static string MatchEval(Match m)
		{
			string type = GetType(m.Value);
			if (type != null)
			{
				return $"<see cref=\"{m.Value}\"/>";
			}
			return m.Value;
		}
	}
}
