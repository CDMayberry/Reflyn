using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Expressions;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class VariableDeclarationStatement : Statement
    {
        public ITypeDeclaration Type { get; }

        public string Name { get; }

        public Expression InitExpression { get; set; }

        public VariableDeclarationStatement(ITypeDeclaration type, string name)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public VariableDeclarationStatement EqualTo(Expression initExpression)
        {
            InitExpression = initExpression;
            return this;
        }

        public VariableDeclaratorSyntax GetDeclarator()
        {
            VariableDeclaratorSyntax declarator = VariableDeclarator(
                Identifier(Name)
            );
            if (InitExpression != null)
            {
                // Expression is expected to be a direct value or variable of some kind.
                declarator = declarator.WithInitializer(
                    EqualsValueClause(
                        InitExpression.ToSyntax()
                    )
                );
            }

            return declarator;
        }

        public VariableDeclarationSyntax GetDeclaration()
        {
            return VariableDeclaration(
                    Type.ToTypeSyntax()
                )
                .WithVariables(
                    SingletonSeparatedList(
                        GetDeclarator()
                    )
                );
        }

        public override StatementSyntax ToSyntax()
        {
            return LocalDeclarationStatement(
                GetDeclaration()
            );
        }

        //public static implicit operator Expression(VariableDeclarationStatement dec) => dec.ToExpr();
        public VariableReferenceExpression ToExpr() => Expr.Var(this);

        // Is problem child with 
        public static implicit operator VariableReferenceExpression(VariableDeclarationStatement stm)
        {
            return new VariableReferenceExpression(stm);
        }

        // Copied from Expression
        public static BinaryOpOperatorExpression operator +(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.Add);
        }

        public static BinaryOpOperatorExpression operator -(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.Subtract);
        }

        public static BinaryOpOperatorExpression operator /(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        public static BinaryOpOperatorExpression operator *(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.Multiply);
        }

        public static BinaryOpOperatorExpression operator %(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.Modulus);
        }

        public static BinaryOpOperatorExpression operator <(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.LessThan);
        }

        public static BinaryOpOperatorExpression operator <=(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.LessThanOrEqual);
        }

        public static BinaryOpOperatorExpression operator >(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.GreaterThan);
        }

        public static BinaryOpOperatorExpression operator >=(VariableDeclarationStatement left, Expression right)
        {
            return new BinaryOpOperatorExpression(left.ToExpr(), right, CodeBinaryOperatorTypeReflyn.GreaterThanOrEqual);
        }
    }
}
