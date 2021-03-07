using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Expressions;
using Reflyn.Mixins;
using Reflyn.Statements;
using Reflyn.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class PropertyDeclaration : MemberDeclaration, ScopeMixin<PropertyDeclaration>
    {
        public ITypeDeclaration Type { get; }

        public StatementList Get { get; } = new StatementList();

        public List<ThrowedExceptionDeclaration> GetExceptions { get; } = new List<ThrowedExceptionDeclaration>();

        public StatementList Set { get; } = new StatementList();

        public List<ThrowedExceptionDeclaration> SetExceptions { get; } = new List<ThrowedExceptionDeclaration>();

        public SyntaxToken? ScopeModifier { get; set; }

        internal PropertyDeclaration(string name, Declaration declaringType, ITypeDeclaration type)
            : base(name, declaringType)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.ToPublic();
        }

        private bool _isAutoProperty = false;
        public PropertyDeclaration ToAutoProperty()
        {
            if (Get.Count > 0 || Set.Count > 0)
            {
                throw new Exception("Property cannot be converted to Auto Property if it already has statements added.");
            }

            _isAutoProperty = true;
            return this;
        }

        public PropertyDeclaration WithGet(Statement statement)
        {
            if (_isAutoProperty)
            {
                throw new Exception("Cannot add statements to a Auto Property.");
            }

            Get.Add(statement);
            return this;
        }

        public PropertyDeclaration WithSet(Statement statement)
        {
            if (_isAutoProperty)
            {
                throw new Exception("Cannot add statements to a Auto Property.");
            }

            Set.Add(statement);
            return this;
        }

        public override string[] GetImports()
        {
            return base.GetImports().Concat(Type.GetImports()).ToArray();
        }

        private AccessorListSyntax AllAccessorsList()
        {
            if (Get.Count <= 0 && Set.Count <= 0)
            {
                throw new Exception("No Getter or Setter assigned for Property.");
            }

            if (Get.Count > 0 && Set.Count > 0)
            {
                return AccessorList(
                    List(
                        new []
                        {
                            AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration
                                )
                                .WithBody(
                                    Block(
                                        Get.ToSyntax()
                                    )
                                ),
                            AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration
                                )
                                .WithBody(
                                    Block(
                                        Set.ToSyntax()
                                    )
                                )

                        }
                    )
                );
            }
            else if (Get.Count > 0)
            {
                return AccessorList(
                    List(
                        new[]
                        {
                            AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration
                                )
                                .WithBody(
                                    Block(
                                        Get.ToSyntax()
                                    )
                                )
                        }
                    )
                );
            }
            else
            {
                return AccessorList(
                    List(
                        new[]
                        {
                            AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration
                                )
                                .WithBody(
                                    Block(
                                        Set.ToSyntax()
                                    )
                                )

                        }
                    )
                );
            }

        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var modifierTokens = GetModifierTokens(ScopeModifier);
            var attributeList = GetCustomAttributes();

            var syntax = PropertyDeclaration(
                    Type.ToTypeSyntax(),
                    Identifier(Name)
                )
                .WithModifiers(modifierTokens)
                .WithAttributeLists(attributeList);

            if (_isAutoProperty)
            {
                return syntax
                    .WithAccessorList(
                        AccessorList(
                            List(
                                new[]{
                                    AccessorDeclaration(
                                            SyntaxKind.GetAccessorDeclaration
                                        )
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)
                                        ),
                                    AccessorDeclaration(
                                            SyntaxKind.SetAccessorDeclaration
                                        )
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)
                                        )
                                }
                            )
                        )
                    );
            }

            return syntax
                .WithAccessorList(AllAccessorsList());
        }

        // Copied from Expression
        public static BinaryOpOperatorExpression operator +(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Add);
        }

        public static BinaryOpOperatorExpression operator -(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Subtract);
        }

        public static BinaryOpOperatorExpression operator /(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        public static BinaryOpOperatorExpression operator /(Expression left, PropertyDeclaration right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        public static BinaryOpOperatorExpression operator *(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Multiply);
        }

        public static BinaryOpOperatorExpression operator %(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Modulus);
        }

        public static BinaryOpOperatorExpression operator <(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThan);
        }

        public static BinaryOpOperatorExpression operator <=(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThanOrEqual);
        }

        public static BinaryOpOperatorExpression operator >(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThan);
        }

        public static BinaryOpOperatorExpression operator >=(PropertyDeclaration left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThanOrEqual);
        }

        public static implicit operator Expression(PropertyDeclaration dec) => dec.ToExpr();
        public Expression ToExpr() => Expr.This.Prop(this);
    }
}
