using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
	public class MethodInvokeExpression : Expression
	{
        public MethodReferenceExpression InvokedMethod { get; }

        public List<Expression> Parameters { get; } = new List<Expression>();

        private readonly bool _conditional;

        public MethodInvokeExpression(MethodReferenceExpression method, bool conditional, params Expression[] parameters)
		{
            InvokedMethod = method ?? throw new ArgumentNullException(nameof(method));
            _conditional = conditional;
			this.Parameters.AddRange(parameters);
        }

        private ArgumentSyntax GetArgument(Expression arg)
        {
            // A little expensive but hopefully not a issue.
            if (arg is ParameterReferenceExpression argExpr)
            {
                return this.GetArgument(argExpr);
            }

            return Argument(arg.ToSyntax());
        }

        private ArgumentSyntax GetArgument(ParameterReferenceExpression arg)
        {
            var argument = Argument(arg.ToSyntax());

            if (arg.Direction == FieldDirectionReflyn.Out)
            {
                // Strangely there is WithRefOrOut but Roslyn Quoter just uses this for out...
                argument = argument
                    .WithRefKindKeyword(
                        Token(SyntaxKind.OutKeyword)
                    );
            }
            else if (arg.Direction == FieldDirectionReflyn.Ref)
            {
                argument = argument
                    .WithRefKindKeyword(
                        Token(SyntaxKind.RefKeyword)
                    );
            }

            return argument;
        }

        private ArgumentListSyntax Arguments()
        {
            if (Parameters.Count <= 0)
            {
                return ArgumentList();
            }

            // Not sure about the Argument(...) call here, wondering if that's handled by the expressions themselves, but will leave it for now.
            //  Update: Leaning toward they don't handle it.
            SyntaxNodeOrToken[] parameters = Parameters
                .Select(x => (SyntaxNodeOrToken)GetArgument(x))
                .Intersperse(Token(SyntaxKind.CommaToken))
                .ToArray();

            return ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    parameters
                )
            );
        }

        public override ExpressionSyntax ToSyntax()
        {
            return _conditional ? this.ConditionalInvoke() : this.Invoke();
        }

        private ExpressionSyntax ConditionalInvoke()
        {
            return ConditionalAccessExpression(
                InvokedMethod.ToSyntax(),
                InvocationExpression(
                    MemberBindingExpression(
                        IdentifierName("Invoke")
                    )
                )
                    .WithArgumentList(
                        this.Arguments()
                    )
            );
        }

        private ExpressionSyntax Invoke()
        {
            return InvocationExpression(
                    InvokedMethod.ToSyntax()
                )
                .WithArgumentList(
                    this.Arguments()
                );
        }
    }
}
