using System;
using System.Xml;

namespace Reflyn.Documentation
{
	public class XmlMarkup
	{
        public XmlDocument Doc { get; } = new XmlDocument();

        public XmlElement Current { get; private set; } = null;

        public XmlElement Root { get; } = null;

        public XmlMarkup(string root)
		{
			if (root == null)
			{
				throw new ArgumentNullException(nameof(root));
			}
			this.Root = Doc.CreateElement(root);
			Current = this.Root;
		}

		public XmlMarkup(XmlDocument doc, XmlElement root)
		{
			this.Doc = doc;
			Current = (this.Root = root);
		}

		public void ResetCursor()
		{
			Current = Root;
		}

		public void Into()
		{
			var xmlElement = Current.FirstChild as XmlElement;
            Current = xmlElement ?? throw new ArgumentException("reached tree boundary");
		}

		public void OutOf()
		{
			Current = (XmlElement)Current.ParentNode;
		}

		public XmlMarkup Add(string name)
		{
			XmlElement newChild = Doc.CreateElement(name);
			Current.AppendChild(newChild);
			return new XmlMarkup(Doc, newChild);
		}

		public XmlMarkup Add(string name, string textFormat, params object[] args)
		{
			XmlElement xmlElement = Doc.CreateElement(name);
			Current.AppendChild(xmlElement);
			XmlText newChild = Doc.CreateTextNode(string.Format(textFormat, args));
			xmlElement.AppendChild(newChild);
			return new XmlMarkup(Doc, xmlElement);
		}

		public void AddText(string textFormat, params object[] args)
		{
			XmlText newChild = Doc.CreateTextNode(string.Format(textFormat, args));
			Current.AppendChild(newChild);
		}

		public XmlMarkup AddCDATA(string name, string textFormat, params object[] args)
		{
			XmlElement xmlElement = Doc.CreateElement(name);
			Current.AppendChild(xmlElement);
			XmlCDataSection newChild = Doc.CreateCDataSection(string.Format(textFormat, args));
			xmlElement.AppendChild(newChild);
			return new XmlMarkup(Doc, xmlElement);
		}

		public XmlComment AddComment(string comment)
		{
			XmlComment xmlComment = Doc.CreateComment(comment);
			Current.AppendChild(xmlComment);
			return xmlComment;
		}

		public XmlAttribute AddAttribute(string name, string textFormat, params object[] args)
		{
			XmlAttribute xmlAttribute = Doc.CreateAttribute(name);
			xmlAttribute.Value = string.Format(textFormat, args);
			Current.Attributes.Append(xmlAttribute);
			return xmlAttribute;
		}
	}
}
