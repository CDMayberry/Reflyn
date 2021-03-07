using System.Collections.Generic;

namespace Reflyn.Declarations
{
    public interface ICustomAttributeProviderDeclaration
    {
        IReadOnlyList<AttributeDeclaration> CustomAttributes { get; }
    }
}
