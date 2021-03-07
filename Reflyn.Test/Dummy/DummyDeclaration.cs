using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Utilities;

namespace Reflyn.Test.Dummy
{
    public class DummyDeclaration : Declaration
    {
        public DummyDeclaration() : base("Dummy", new NameConformer())
        {

        }

        public DummyDeclaration(string name, NameConformer conformer) : base(name, conformer)
        {
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            throw new System.NotImplementedException();
        }
    }
}
