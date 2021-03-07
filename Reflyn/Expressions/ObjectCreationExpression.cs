using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Declarations;
using Reflyn.Statements;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class ObjectCreationExpression : Expression
    {
        protected readonly ITypeDeclaration Type;

        // TODO: need to add way to add new Args after constructor runs.
        private readonly ExpressionList _args = new ExpressionList();
        private readonly List<AssignStatement> _initializers = new List<AssignStatement>();

        public ObjectCreationExpression(ITypeDeclaration type, params Expression[] args)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this._args.AddRange(args);
        }

        protected ArgumentListSyntax Arguments()
        {
            if (_args.Count <= 0)
            {
                return ArgumentList();
            }

            SyntaxNodeOrToken[] parameters = _args.Select(x => (SyntaxNodeOrToken)Argument(x.ToSyntax())).Intersperse(Token(SyntaxKind.CommaToken)).ToArray();

            return ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    parameters
                )
            );
        }

        public ObjectCreationExpression AddInitializer(string name, Expression expression)
        {
            _initializers
                .Add(Stm.Assign(Expr.Var(name), expression));
            return this;
        }

        public override ExpressionSyntax ToSyntax()
        {
            var result = ObjectCreationExpression(
                    Type.ToTypeSyntax()
                )
                .WithArgumentList(
                    Arguments()
                );

            if (_initializers.Count > 0)
            {
                result =
                    result.WithInitializer(
                        InitializerExpression(
                            SyntaxKind.ObjectInitializerExpression,
                            SeparatedList<ExpressionSyntax>(
                                new SyntaxNodeOrTokenList(_initializers.Select(x => (SyntaxNodeOrToken)x.ToExpressionSyntax())).Intersperse(Token(SyntaxKind.CommaToken))
                                )
                        )
                    );
            }

            return result;

        }
    }
}
