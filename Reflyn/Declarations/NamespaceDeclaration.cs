using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class NamespaceDeclaration : Declaration
    {
        private readonly HashSet<string> _imports = new HashSet<string>();

        public NamespaceDeclaration Parent { get; set; }

        public Dictionary<string, NamespaceDeclaration> Namespaces { get; } = new Dictionary<string, NamespaceDeclaration>();

        public Dictionary<string, ClassDeclaration> Classes { get; } = new Dictionary<string, ClassDeclaration>();

        public Dictionary<string, EnumDeclaration> Enums { get; } = new Dictionary<string, EnumDeclaration>();

        public override string FullName
        {
            get
            {
                if (Parent != null)
                {
                    return $"{Parent.FullName}.{base.Name}";
                }
                return base.Name;
            }
        }

        public virtual IReadOnlyCollection<string> Imports => _imports;

        public NamespaceDeclaration(string name)
            : base(name, new NameConformer())
        {
            _imports.Add("System");
        }

        public NamespaceDeclaration(string name, NameConformer conformer)
            : base(name, conformer)
        {
            _imports.Add("System");
        }

        public NamespaceDeclaration AddNamespace(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (Namespaces.ContainsKey(name))
            {
                throw new ApplicationException("namespace already created");
            }

            var namespaceDeclaration = new NamespaceDeclaration(base.Conformer.ToCapitalized(name), base.Conformer)
                {
                    Parent = this
                };
            Namespaces.Add(name, namespaceDeclaration);
            return namespaceDeclaration;
        }

        public NamespaceDeclaration AddImport(string name)
        {
            _imports.Add(name);
            return this;
        }

        public EnumDeclaration AddEnum(string name, bool flags)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (Enums.ContainsKey(name))
            {
                throw new ArgumentException("enum already present");
            }
            var enumDeclaration = new EnumDeclaration(name, this, flags);
            Enums.Add(name, enumDeclaration);
            return enumDeclaration;
        }

        public ClassDeclaration AddClass(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (Classes.ContainsKey(name))
            {
                throw new ArgumentException("class already existing in namespace");
            }
            var classDeclaration = new ClassDeclaration(name, this);
            Classes.Add(name, classDeclaration);
            return classDeclaration;
        }

        public ClassDeclaration AddStruct(string name)
        {
            return AddClass(name).ToStruct();
        }

        public ClassDeclaration AddInterface(string name)
        {
            return AddClass(name).ToInterface();
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var result = NamespaceDeclaration(
                IdentifierName(FullName)
            );

            if (Classes.Count > 0)
            {
                result = result.AddMembers(
                    Classes.Values.Select(x => x.ToSyntax()).ToArray()
                );
            }

            if (Enums.Count > 0)
            {
                result = result.AddMembers(
                    Enums.Values.Select(x => x.ToSyntax()).ToArray()
                );
            }

            return result;
        }

        public CompilationUnitSyntax ToCompilationUnit()
        {
            return CompilationUnit()
                .WithUsings(
                    GetImportSyntaxList()
                )
                .WithMembers(
                    // Namespaces
                    SingletonList(
                        ToSyntax()
                    )
                );
        }

        private SyntaxList<UsingDirectiveSyntax> GetImportSyntaxList()
        {
            // Copy to list
            var imports = new HashSet<string>(_imports);

            foreach (var clazz in Classes)
            {
                foreach (var import in clazz.Value.GetImports())
                {
                    imports.Add(import);
                }
            }

            return List(imports.Select(x => UsingDirective(ReflynUtilities.GetNameSyntax(x))));
        }
    }
}
