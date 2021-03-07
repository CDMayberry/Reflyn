using System;

namespace Reflyn.Declarations
{
    public class StringTypeDeclaration : ITypeDeclaration
    {
        public string Name { get; }
        public static readonly string[] Default = {};
        private readonly string[] _imports;

        public string FullName => Name;

        internal StringTypeDeclaration(string typeName)
        {
            this.Name = typeName ?? throw new ArgumentNullException(nameof(typeName));
            this._imports = Default;
        }

        // TODO: I'd like to force devs to always add a Imports array, then they should also have custom StringTypeDeclarations saved somewhere to be reused.
        public StringTypeDeclaration(string typeName, string[] imports)
        {
            this.Name = typeName ?? throw new ArgumentNullException(nameof(typeName));
            if (imports == null || imports.Length <= 0)
            {
                throw new ArgumentException("Imports but contain at least one namespace.");
            }
            
            this._imports = imports;
        }

        public string[] GetImports()
        {
            // Nothing we can import
            return _imports;
        }
    }
}
