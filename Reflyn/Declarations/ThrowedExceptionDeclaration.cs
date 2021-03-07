using System;

namespace Reflyn.Declarations
{
    public class ThrowedExceptionDeclaration
    {
        public Type ExceptionType { get; }

        public string Description { get; }

        public ThrowedExceptionDeclaration(Type type, string description)
        {
            this.ExceptionType = type;
            this.Description = description;
        }
    }
}
