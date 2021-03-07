using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class AttributeArgument
    {
        private Expression _value;

        public string Name { get; }

        public Expression Value
        {
            get => _value;
            set => this._value = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal AttributeArgument(string name, Expression value)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this._value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string[] GetImports()
        {
            return Value.GetImports();
        }

        public AttributeArgumentSyntax ToSyntax()
        {
            // This refers to the default argument, IE [MenuItem("Tools/Test")]
            // TODO: Remove string check and either have 2 constructors with a flag or two separate AttributeArgument classes.
            if (Name.StartsWith("__Positional"))
            {
                return AttributeArgument(
                    Value.ToSyntax()
                );
            }

            
            // This refers to any named arguments, IE [MenuItem(version = 1)]
            return AttributeArgument(
                Value.ToSyntax()
            )
            .WithNameEquals(
                NameEquals(
                    IdentifierName(Name)
                )
            );
        }
    }
}
