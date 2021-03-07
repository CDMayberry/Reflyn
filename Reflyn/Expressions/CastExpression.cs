using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class CastExpression : Expression
    {
        private readonly ITypeDeclaration _targetType;

        private readonly Expression _expression;

        public CastExpression(ITypeDeclaration targetType, Expression expression)
        {
            this._targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this._expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override string[] GetImports()
        {
            return _targetType.GetImports().Concat(_expression.GetImports()).ToArray();
        }

        public override ExpressionSyntax ToSyntax()
        {
            return CastExpression(
                _targetType.ToTypeSyntax(),
                _expression.ToSyntax()
            );
        }
    }
}
