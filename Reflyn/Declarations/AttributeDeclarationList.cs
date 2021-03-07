using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class AttributeDeclarationList : List<AttributeDeclaration>
    {
        public virtual AttributeDeclaration Add(ITypeDeclaration type)
        {
            var attributeDeclaration = new AttributeDeclaration(type);
            base.Add(attributeDeclaration);
            return attributeDeclaration;
        }

        public virtual AttributeDeclaration Add(Type type)
        {
            return Add(new TypeTypeDeclaration(type));
        }

        public virtual AttributeDeclaration Add(string type)
        {
            return Add(new StringTypeDeclaration(type));
        }
        public virtual string[] GetImports()
        {
            return this.SelectMany(x => x.GetImports()).ToArray();
        }

        public AttributeListSyntax[] ToSyntax()
        {
            return this.Select(x =>
                AttributeList(SingletonSeparatedList(x.ToSyntax()))
            ).ToArray();
        }
    }
}
