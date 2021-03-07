using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
	public class MethodReferenceExpression : Expression
	{
        public Expression Target { get; }

		public string Name { get; }

        public ExpressionList TypeParameters { get; } = new ExpressionList();

        public MethodReferenceExpression(Expression target, string method)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Name = method ?? throw new ArgumentNullException(nameof(method));
        }

        public MethodReferenceExpression(Expression target, string method, params Expression[] typeExpressions)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Name = method ?? throw new ArgumentNullException(nameof(method));
            this.TypeParameters.AddRange(typeExpressions);
        }

        public static MethodReferenceExpression From(Expression target, MethodDeclaration dec)
        {
            return new MethodReferenceExpression(target, dec.Name);
        }

        public static MethodReferenceExpression From(Expression target, EventDeclaration dec) 
        {
            return new MethodReferenceExpression(target, dec.Name);
        }

        private TypeArgumentListSyntax TypeArguments()
        {
            if (TypeParameters.Count <= 0)
            {
                return TypeArgumentList();
            }

            // Not sure about the Argument(...) call here, wondering if that's handled by the expressions themselves, but will leave it for now.
            //  Update: Leaning toward they don't handle it.
            SyntaxNodeOrToken[] parameters = TypeParameters.Select(x => (SyntaxNodeOrToken)x.ToSyntax()).Intersperse(Token(SyntaxKind.CommaToken)).ToArray();

            return TypeArgumentList(
                SeparatedList<TypeSyntax>(
                    parameters
                )
            );
        }

        private SimpleNameSyntax GetName()
        {
            if (TypeParameters.Count > 0)
            {
                return GenericName(Name).WithTypeArgumentList(TypeArguments());
            }

            return IdentifierName(Name);
        }

        // Calling ToSyntax would build the whole body, instead we just reference the name. NOTE: This doesn't actually call this method, that is what Invoke does.
        public override ExpressionSyntax ToSyntax()
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                Target.ToSyntax(),
                GetName()
            );
        }

        public MethodInvokeExpression Invoke()
		{
			return new MethodInvokeExpression(this, false);
        }

        public MethodInvokeExpression Invoke(params Expression[] args)
        {
            return new MethodInvokeExpression(this, false,
                args
            );
        }

        public MethodInvokeExpression Invoke(params ParameterReferenceExpression[] args)
		{
			return new MethodInvokeExpression(this, false, args);
		}

		public MethodInvokeExpression Invoke(params ParameterDeclaration[] parameters)
		{
			var array = new ParameterReferenceExpression[checked((uint)parameters.Length)];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = Expr.Arg(parameters[i]);
			}
			return Invoke(array);
        }

        public MethodInvokeExpression ConditionalInvoke()
        {
            return new MethodInvokeExpression(this, true);
        }

        public MethodInvokeExpression ConditionalInvoke(params Expression[] args)
        {
            return new MethodInvokeExpression(this, true,
                    args
                );
        }

        // I don't love that it has to be a ArgumentReferenceExpression, but will see if it works for now.
        public MethodInvokeExpression ConditionalInvoke(params ParameterReferenceExpression[] args)
        {
            return new MethodInvokeExpression(this, true, args);
        }

        public MethodInvokeExpression ConditionalInvoke(params ParameterDeclaration[] parameters)
        {
            var array = new ParameterReferenceExpression[checked((uint)parameters.Length)];
            for (int i = 0; i < parameters.Length; i++)
            {
                array[i] = Expr.Arg(parameters[i]);
            }
            return ConditionalInvoke(array);
        }
	}
}
