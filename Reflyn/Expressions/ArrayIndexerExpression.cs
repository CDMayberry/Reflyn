using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    // Array Items must be ints, regular items could be strings or whatever a dictionary uses.
    public class ArrayIndexerExpression : Expression
    {
        public Expression TargetExpression { get; }

        public Expression IndexExpression { get; }

        public ArrayIndexerExpression(Expression targetExpression)
        {
            this.TargetExpression = targetExpression ?? throw new ArgumentNullException(nameof(targetExpression));
        }

        public ArrayIndexerExpression(Expression targetExpression, Expression index)
            : this(targetExpression)
        {
            this.IndexExpression = index;
        }

        public override ExpressionSyntax ToSyntax()
        {
            return ElementAccessExpression(
                    TargetExpression.ToSyntax()
                )
                .WithArgumentList(
                    BracketedArgumentList(
                        SingletonSeparatedList<ArgumentSyntax>(
                            Argument(
                                IndexExpression.ToSyntax()
                            )
                        )
                    )
                );
        }
    }
}
