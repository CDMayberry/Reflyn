using System;
using Reflyn.Declarations;

namespace Reflyn.Documentation
{
	public class Documentation
	{
		private readonly XmlMarkup doc = new XmlMarkup("doc");

		private XmlMarkup summary = null;

		private XmlMarkup remarks = null;

		public bool HasSummary => summary != null;

		public XmlMarkup Summary
		{
			get
			{
				if (summary == null)
				{
					summary = doc.Add("summary");
				}
				return summary;
			}
		}

		public bool HasRemarks => remarks != null;

		public XmlMarkup Remarks => remarks = remarks ?? doc.Add("remarks");

        public void AddException(Type t, string description)
		{
			XmlMarkup xmlMarkup = doc.Add("exception", TypeFinder.CrossRef(description));
			xmlMarkup.AddAttribute("cref", t.FullName);
		}

		public void AddException(ThrowedExceptionDeclaration ex)
		{
			AddException(ex.ExceptionType, ex.Description);
		}

		public XmlMarkup AddParam(ParameterDeclaration para)
		{
			XmlMarkup xmlMarkup = doc.Add("param");
			xmlMarkup.AddAttribute("name", para.Name);
			return xmlMarkup;
		}

		public void AddInclude(string file, string path)
		{
			XmlMarkup xmlMarkup = doc.Add("include");
			xmlMarkup.AddAttribute("file", file);
			xmlMarkup.AddAttribute("path", path);
		}

		/*public void ToCodeDom(CodeCommentStatementCollection comments)
		{
			foreach (XmlElement childNode in doc.Root.ChildNodes)
			{
				var stringWriter = new StringWriter();
                var xmlTextWriter = new XmlTextWriter(stringWriter)
                {
                    Formatting = Formatting.Indented
                };
                childNode.WriteTo(xmlTextWriter);
				xmlTextWriter.Flush();
				string[] array = stringWriter.ToString().Split('\r');
				foreach (string text in array)
				{
					if (text.Length > 1)
					{
						if (text[0] == '\n')
						{
							comments.Add(new CodeCommentStatement(text.Substring(1, text.Length - 1), docComment: true));
						}
						else
						{
							comments.Add(new CodeCommentStatement(text, docComment: true));
						}
					}
				}
			}
		}*/
	}
}
