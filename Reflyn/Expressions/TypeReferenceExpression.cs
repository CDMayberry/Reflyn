using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;

namespace Reflyn.Expressions
{
    public class TypeReferenceExpression : Expression
    {
        public ITypeDeclaration Type { get; }

        public TypeReferenceExpression(ITypeDeclaration type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override string[] GetImports()
        {
            return Type.GetImports();
        }

        public override ExpressionSyntax ToSyntax()
        {
            return Type.ToTypeSyntax();
        }
    }
}
