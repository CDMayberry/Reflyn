using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class MethodSignature
    {
        public ParameterDeclarationList Parameters { get; } = new ParameterDeclarationList();

        public ITypeDeclaration ReturnType { get; set; } = new TypeTypeDeclaration(typeof(void));

        public ParameterReferenceExpression this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                foreach (var arg in Parameters)
                {
                    if (arg.Name == name)
                    {
                        return new ParameterReferenceExpression(arg);
                    }
                }
                throw new ArgumentException("could not find " + name);
            }
        }

        public string[] GetImports()
        {
            return ReturnType.GetImports().Concat(Parameters.GetImports()).ToArray();
        }

        public ParameterListSyntax ToSyntax()
        {
            // Array elements will already be Parameter(...), unlike Arguments
            SyntaxNodeOrToken[] parameters = Parameters.ToSyntaxArray();

            return ParameterList(
                SeparatedList<ParameterSyntax>(
                    parameters
                )
            );
        }
    }
}
