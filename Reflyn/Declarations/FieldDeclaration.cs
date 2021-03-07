using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using Reflyn.Mixins;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    // TODO: Can we create a inheriting type for FieldArray so it can have shortcuts to common methods / props?

    public class FieldDeclaration : MemberDeclaration, ScopeMixin<FieldDeclaration>, StaticMixin<FieldDeclaration>, ReadOnlyMixin<FieldDeclaration>, ConstMixin<FieldDeclaration>
    {
        public ITypeDeclaration Type { get; }

        public Expression InitExpression { get; set; }

        public SyntaxToken? ScopeModifier { get; set; }
        public SyntaxToken? ConstModifier { get; set; }
        public SyntaxToken? ReadOnlyModifier { get; set; }
        public SyntaxToken? StaticModifier { get; set; }

        internal FieldDeclaration(string name, Declaration declaringType, ITypeDeclaration type) : base(name, declaringType)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override string[] GetImports()
        {
            return base.GetImports().Concat(Type.GetImports()).ToArray();
        }

        public FieldDeclaration EqualTo(Expression expr)
        {
            InitExpression = expr;
            return this;
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var attributeList = GetCustomAttributes();

            return FieldDeclaration(
                VariableDeclaration(
                    Type.ToTypeSyntax()
                )
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(Name)
                                .WithInitializer(InitExpression != null ? EqualsValueClause(InitExpression.ToSyntax()) : null)
                        )
                    )
            )
                .WithModifiers(GetModifierTokens())
                .WithAttributeLists(attributeList);
        }

        protected SyntaxTokenList GetModifierTokens()
        {
            if (ConstModifier != null && (StaticModifier != null || ReadOnlyModifier != null))
            {
                throw new Exception("A field cannot be marked with Const and Static or ReadOnly.");
            }
            
            // Const wins over Static/Readonly, but might be better to throw a exception if both are set... Setting both is definitely a error.
            if (ConstModifier != null)
            {
                return GetModifierTokens(ScopeModifier, ConstModifier);
            }

            return GetModifierTokens(ScopeModifier, StaticModifier, ReadOnlyModifier);
        }

        //Be careful with this...
        public static implicit operator Expression(FieldDeclaration dec) => dec.ToExpr();

        // Copied from Expression
        public static BinaryOpOperatorExpression operator +(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Add);
        }

        public static BinaryOpOperatorExpression operator -(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Subtract);
        }

        public static BinaryOpOperatorExpression operator /(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        public static BinaryOpOperatorExpression operator /(Expression left, FieldDeclaration right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        public static BinaryOpOperatorExpression operator *(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Multiply);
        }

        public static BinaryOpOperatorExpression operator %(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Modulus);
        }

        public static BinaryOpOperatorExpression operator <(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThan);
        }

        public static BinaryOpOperatorExpression operator <=(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThanOrEqual);
        }

        public static BinaryOpOperatorExpression operator >(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThan);
        }

        public static BinaryOpOperatorExpression operator >=(FieldDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThanOrEqual);
        }

        public Expression ToExpr() => Expr.This.Field(this);
    }
}
