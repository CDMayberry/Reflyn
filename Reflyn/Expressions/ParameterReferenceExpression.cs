using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ParameterReferenceExpression : Expression
    {
        //public ParameterDeclaration Parameter { get; }
        public string Name { get; }
        public Expression Expr { get; }
        // Copied from ParameterDeclaration which I don't like,
        // is there any shortcut creating a ParameterDeclaration rather than simply using a string here?
        public FieldDirectionReflyn Direction { get; }

        // TODO: Could we just pass in a Type instead? Like how most other Expression types do?
        public ParameterReferenceExpression(ParameterDeclaration parameter)
        {
            //this.Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
            this.Name = parameter?.Name ?? throw new ArgumentNullException(nameof(parameter));
            this.Direction = parameter.Direction;
        }

        public ParameterReferenceExpression(VariableReferenceExpression variable, FieldDirectionReflyn direction)
        {
            this.Name = variable?.Name ?? throw new ArgumentNullException(nameof(variable));
            this.Direction = direction;
        }

        public ParameterReferenceExpression(Expression expr, FieldDirectionReflyn direction)
        {
            this.Expr = expr ?? throw new ArgumentNullException(nameof(expr));
            this.Direction = direction;
        }

        public ParameterReferenceExpression(Declaration dec, FieldDirectionReflyn direction)
        {
            this.Name = dec?.Name ?? throw new ArgumentNullException(nameof(dec));
            this.Direction = direction;
        }

        public ParameterReferenceExpression(string parameter, FieldDirectionReflyn direction)
        {
            this.Name = parameter ?? throw new ArgumentNullException(nameof(parameter));
            this.Direction = direction;
        }

        public override ExpressionSyntax ToSyntax()
        {
            return Expr?.ToSyntax() ?? IdentifierName(Name);
        }
    }
}
