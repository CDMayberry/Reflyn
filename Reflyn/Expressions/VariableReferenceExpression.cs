using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Statements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        public readonly string Name;

        public VariableReferenceExpression(VariableDeclarationStatement declaration)
        {
            this.Name = declaration?.Name ?? throw new ArgumentNullException(nameof(declaration));
        }
        public VariableReferenceExpression(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        // Not much else to do here really...
        public override ExpressionSyntax ToSyntax()
        {
            return IdentifierName(Name);
        }
    }
}
