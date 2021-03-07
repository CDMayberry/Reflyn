using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    // TODO: Should probably require that Expression is a SnippetExpression OR build the string manually?
    public class NativeMethodReferenceExpression : Expression
    {
        public Expression Target { get; }

        public string Name { get; }

        public NativeMethodReferenceExpression(Expression target, string name)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public NativeMethodInvokeExpression Invoke()
        {
            return new NativeMethodInvokeExpression(this);
        }

        public NativeMethodInvokeExpression Invoke(params Expression[] args)
        {
            return new NativeMethodInvokeExpression(this, args);
        }

        public NativeMethodInvokeExpression Invoke(params ParameterDeclaration[] parameters)
        {
            var array = new ParameterReferenceExpression[checked((uint)parameters.Length)];
            for (int i = 0; i < parameters.Length; i++)
            {
                array[i] = Expr.Arg(parameters[i]);
            }
            return Invoke(array);
        }

        public override ExpressionSyntax ToSyntax()
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ThisExpression(),
                IdentifierName(Name)
            );
        }
    }
}
