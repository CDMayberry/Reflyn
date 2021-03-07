using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Expressions;
using Reflyn.Mixins;
using Reflyn.Statements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class MethodDeclaration : MemberDeclaration, StaticMixin<MethodDeclaration>, MethodAccessMixin<MethodDeclaration>, ScopeMixin<MethodDeclaration>, ParametersMixin<MethodDeclaration>, AsyncMixin<MethodDeclaration>
    {
        public MethodSignature Signature { get; } = new MethodSignature();

        public StatementList Body { get; set; } = new StatementList();

        public ParameterDeclaration[] Parameters => Signature.Parameters.ToArray();

        public SyntaxToken? AccessModifier { get; set; }
        public SyntaxToken? StaticModifier { get; set; }
        public SyntaxToken? ScopeModifier { get; set; }
        public SyntaxToken? AsyncModifier { get; set; }

        internal MethodDeclaration(string name, Declaration declaringType)
            : base(name, declaringType)
        {

        }

        public override string[] GetImports()
        {
            // TODO: Do we need to add Body and go to each statements' expression(s) and get their imports?
            return base.GetImports().Concat(Signature.GetImports()).ToArray();
        }

        public MethodDeclaration SetReturnType(Type type)
        {
            return SetReturnType(new TypeTypeDeclaration(type));
        }

        public MethodDeclaration SetReturnType(string type)
        {
            return SetReturnType(new StringTypeDeclaration(type));
        }

        public MethodDeclaration SetReturnType(ITypeDeclaration type)
        {
            Signature.ReturnType = type;
            return this;
        }
        
        public MethodDeclaration AddAssign(Expression left, Expression right)
        {
            Body.Add(Stm.Assign(left, right));
            return this;
        }

        public MethodDeclaration AddReturn(Expression expr)
        {
            Body.Add(Stm.Return(expr));
            return this;
        }

        public MethodDeclaration Add(Statement statement)
        {
            Body.Add(statement);
            return this;
        }

        public MethodDeclaration Add(Statement statement, params Statement[] statements)
        {
            Body.Add(statement);
            if (statements.Length > 0)
            {
                Body.AddRange(statements);
            }
            return this;
        }

        // TODO: Can we determine who owns the current MethodDeclaration? IE The current class, or a method on a VariableDeclaration?

        public MethodInvokeExpression InvokeOnThis()
        {
            return Expr.This.Method(this).Invoke();
        }

        public MethodInvokeExpression InvokeOnThis(params string[] expr)
        {
            return Expr.This.Method(this).Invoke(expr.Select(Expr.Var).ToArray<Expression>());
        }

        public MethodInvokeExpression InvokeOnThis(params Expression[] expr)
        {
            return Expr.This.Method(this).Invoke(expr);
        }

        public MethodInvokeExpression InvokeOnThis(params ParameterReferenceExpression[] expr)
        {
            return Expr.This.Method(this).Invoke(expr);
        }

        public MethodInvokeExpression InvokeOnThis(params ParameterDeclaration[] parameters)
        {
            var array = new ParameterReferenceExpression[checked((uint)parameters.Length)];
            for (int i = 0; i < parameters.Length; i++)
            {
                array[i] = Expr.Arg(parameters[i]);
            }
            return InvokeOnThis(array);
        }

        /// <summary>
        /// Shortcut to add a Statement that calls the Base method version of this method using the defined Parameters.
        /// </summary>
        /// <returns></returns>
        public MethodDeclaration CallBaseWithParameters()
        {
            return Add(Expr.Base.Method(this).Invoke(Parameters));
        }
        
        public override MemberDeclarationSyntax ToSyntax()
        {
            if (StaticModifier != null && AccessModifier != null)
            {
                throw new Exception("Method " + Name + " cannot be both static and have a access modifier.");
            }

            if (AsyncModifier != null && AccessModifier == Token(SyntaxKind.AbstractKeyword))
            {
                throw new Exception("Method " + Name + " cannot be both async and abstract.");
            }

            // TODO: Static and (abstract/virtual) cannot be used together, async requires a body and thus cannot be abstract. Add guards against using them together.
            var result = MethodDeclaration(
                    Signature.ReturnType.ToTypeSyntax(),
                    GetIdentifierSyntaxToken()
                )
                .WithModifiers(
                    GetModifierTokens(ScopeModifier, StaticModifier, AccessModifier, AsyncModifier) 
                )
                .WithAttributeLists(GetCustomAttributes())
                .WithParameterList(
                    Signature.ToSyntax()
                );

            if (AccessModifier != Token(SyntaxKind.AbstractKeyword))
            {
                result = result
                    .WithBody(
                        Block(Body.ToSyntax())
                    );
            }
            else
            {
                result = result
                    .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken)
                    );
            }

            return result;
        }
    }
}
