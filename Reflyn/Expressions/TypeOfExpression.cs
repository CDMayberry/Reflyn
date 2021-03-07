using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Expressions
{
    public class TypeOfExpression : Expression
    {
        private readonly ITypeDeclaration _targetType;

        public TypeOfExpression(ITypeDeclaration targetType)
        {
            this._targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public override string[] GetImports()
        {
            return _targetType.GetImports();
        }

        public override ExpressionSyntax ToSyntax()
        {
            return TypeOfExpression(
                _targetType.ToTypeSyntax()
            );
        }
    }
}
