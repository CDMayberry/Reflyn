using Reflyn.Declarations;

namespace Reflyn.Refly.Templates
{
	public interface ITemplate
	{
		string TemplateName { get; }

		NamespaceDeclaration NamespaceDeclaration { get; set; }

		void Generate();
	}
}
