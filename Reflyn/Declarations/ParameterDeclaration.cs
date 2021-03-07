using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    // Hm, why doesn't this inherit from a base Declaration?
	public class ParameterDeclaration : ICustomAttributeProviderDeclaration
	{
        public string Name { get; }

        public StringWriter Description { get; } = new StringWriter();

        // TODO: Add to ToSyntax
        public bool IsNonNull { get; }

        public ITypeDeclaration Type { get; }

        // TODO: Add to ToSyntax
        public FieldDirectionReflyn Direction { get; set; } = FieldDirectionReflyn.None;

        // TODO: Add to ToSyntax
        protected AttributeDeclarationList _customAttributes { get; } = new AttributeDeclarationList();

        public IReadOnlyList<AttributeDeclaration> CustomAttributes => _customAttributes;

        internal ParameterDeclaration(ITypeDeclaration type, string name, bool nonNull, FieldDirectionReflyn direction)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Direction = direction;
            this.IsNonNull = nonNull;
        }

        // Used by AddSimple, primarily for converting to args.
        internal ParameterDeclaration(string name, bool nonNull, FieldDirectionReflyn direction)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Direction = direction;
            this.IsNonNull = nonNull;
        }

        internal ParameterDeclaration(string name, bool nonNull)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.IsNonNull = nonNull;
        }

        public string[] GetImports()
        {
            var imports = new List<string>(Type.GetImports());
            foreach (var attr in CustomAttributes)
            {
                imports.AddRange(attr.AttributeType.GetImports());
            }

            return imports.ToArray();
        }

        // TODO: Figure if we need `SyntaxNodeOrToken ToSyntax()` or if we can have this be ParameterSyntax only.
        public ParameterSyntax ToParameterSyntax()
        {
            // TODO: PredefinedType() for common C# types, could work that in perhaps.
            var param = Parameter(
                Identifier(Name)
            );

            if (Type != null)
            {
                param = param
                    .WithType(
                        Type.ToTypeSyntax()
                    );
            }

            if (Direction != FieldDirectionReflyn.None)
            {

                param = param
                    .WithModifiers(
                        TokenList(
                            Direction.ToToken()
                        )
                    );
            }

            return param;
        }

        public SyntaxNodeOrToken ToSyntax()
        {
            return ToParameterSyntax();
        }

        public CatchDeclarationSyntax ToCatchSyntax()
        {
            // TODO: PredefinedType() for common C# types, could work that in perhaps.
            return CatchDeclaration(
                    IdentifierName(Type.FullName)
                )
                .WithIdentifier(
                    Identifier(Name)
                );
        }
	}
}
