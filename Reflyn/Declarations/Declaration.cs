using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

[assembly: InternalsVisibleTo("Reflyn.Test")]
namespace Reflyn.Declarations
{
    // TODO: Separate this from Declarations that have visibility. Remove MemberAttributes / CustomAttributes.
    // Reason: Namespaces don't have public/private, therefore this information is too low.
    public abstract class Declaration : ITypeDeclaration
    {
        public string Name { get; }
        public virtual string[] GetImports()
        {
            var imports = new List<string>();
            foreach (var attr in CustomAttributes)
            {
                imports.AddRange(attr.AttributeType.GetImports());
            }

            return imports.ToArray();
        }

        public virtual string FullName => Name;

        public NameConformer Conformer { get; }

        public Documentation.Documentation Doc { get; } = new Documentation.Documentation();

        protected AttributeDeclarationList _customAttributes { get; } = new AttributeDeclarationList();

        public IReadOnlyList<AttributeDeclaration> CustomAttributes => _customAttributes;

        //public CodeTypeReference TypeReference => new CodeTypeReference(Name);

        internal Declaration(string name, NameConformer conformer)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Conformer = conformer ?? throw new ArgumentNullException(nameof(conformer));
        }

        protected SyntaxList<AttributeListSyntax> GetCustomAttributes()
        {
            return List(_customAttributes.ToSyntax());
        }

        /*protected void ToCodeDom(CodeTypeMember ctm)
        {
            throw new NotImplementedException();
            /*ctm.Attributes = Attributes;
            ctm.Name = Name;
            Doc.ToCodeDom(ctm.Comments);
            ctm.CustomAttributes.AddRange(_customAttributes.ToCodeDom());#1#
        }*/

        public AttributeDeclaration AddCustomAttribute<TA>() where TA : Attribute
        {
            return AddCustomAttribute(typeof(TA));
        }

        public AttributeDeclaration AddCustomAttribute(Type type)
        {
            if (!type.IsSubclassOf(typeof(Attribute)))
            {
                throw new Exception("Type is not a Attribute.");
            }

            return _customAttributes.Add(type);
        }

        protected SyntaxToken GetIdentifierSyntaxToken() => Identifier(Name);

        //public abstract CodeTypeMember ToCodeDom();

        public abstract MemberDeclarationSyntax ToSyntax();
        
        protected static SyntaxTokenList GetModifierTokens(SyntaxToken? firstToken, params SyntaxToken?[] tokens)
        {
            SyntaxTokenList modifierTokens = new SyntaxTokenList().AddNotNull(firstToken);

            foreach (var token in tokens)
            {
                modifierTokens = modifierTokens.AddNotNull(token);
            }

            return modifierTokens;
        }
    }

    public static class DeclarationExtensions
    {
        // Individual classes would need to implement this, as we T can't be inferred here.
        /*public static T WithCustomAttribute<T, TA>(this T dec) where T : Declaration where TA : Attribute
        {
            dec.AddCustomAttribute<TA>();
            return dec;
        }*/

        public static T WithCustomAttribute<T>(this T dec, Type type) where T : Declaration
        {
            dec.AddCustomAttribute(type);
            return dec;
        }
    }
}
