using System;

namespace Reflyn.Declarations
{
    public class TypeTypeDeclaration : ITypeDeclaration
    {
        public readonly Type Type;

        public string Name => Type.Name;
        public string[] GetImports()
        {
            // TODO: Should probably add some kind of logging if this is null. Null means it isn't in a namespace.
            return Type.Namespace != null
                ? new[] { Type.Namespace }
                : new string[] { };
        }

        public string FullName => Type.FullName;

        public TypeTypeDeclaration(Type type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
