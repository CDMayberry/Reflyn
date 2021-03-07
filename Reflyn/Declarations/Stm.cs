using System;
using Reflyn.Expressions;
using Reflyn.Statements;
using Reflyn.Utilities;

namespace Reflyn.Declarations
{
    public static class Stm
    {
        public static AssignStatement Assign(Expression left, Expression right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            return new AssignStatement(left, right);
        }

        public static ExpressionStatement ToStm(Expression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new ExpressionStatement(expr);
        }

        public static ExpressionStatement FromExpr(Expression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new ExpressionStatement(expr);
        }

        public static MethodReturnStatement Return(Expression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new MethodReturnStatement(expr);
        }

        public static MethodReturnStatement Return()
        {
            return new MethodReturnStatement();
        }

        public static BreakStatement Break => new BreakStatement();
        public static ContinueStatement Continue => new ContinueStatement();

        public static NativeStatement Comment(string comment)
        {
            throw new NotImplementedException();
            /*if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }
            return new NativeStatement(new CodeCommentStatement(comment, docComment: false));*/
        }

        public static UsingStatement Using(string varName, Expression assignment, params Statement[] statements)
        {
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(assignment));
            }
            return new UsingStatement(varName, assignment, statements);
        }

        public static ConditionStatement If(Expression condition, params Statement[] trueStatements)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            return new ConditionStatement(condition, trueStatements);
        }

        public static ConditionStatement IfIdentity(Expression left, Expression right, params Statement[] trueStatements)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            return If(left.Identity(right), trueStatements);
        }

        public static ConditionStatement ThrowIfNull(Expression condition, Expression toThrow)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (toThrow == null)
            {
                throw new ArgumentNullException(nameof(toThrow));
            }
            return IfNull(condition, Throw(toThrow));
        }

        public static ConditionStatement ThrowIfNull(Expression condition, string message)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return ThrowIfNull(condition, Expr.New(typeof(NullReferenceException), Expr.Prim(message)));
        }

        public static ConditionStatement ThrowIfNull(ParameterDeclaration param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            return ThrowIfNull(Expr.Arg(param), Expr.New(typeof(ArgumentNullException), Expr.Prim(param.Name)));
        }

        public static ConditionStatement IfNotIdentity(Expression left, Expression right, params Statement[] trueStatements)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            return If(left.NotIdentity(right), trueStatements);
        }

        public static ConditionStatement IfNull(Expression left, params Statement[] trueStatements)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            return IfIdentity(left, Expr.Null, trueStatements);
        }

        public static ConditionStatement IfNotNull(Expression left, params Statement[] trueStatements)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            return IfNotIdentity(left, Expr.Null, trueStatements);
        }

        public static NativeStatement Snippet(string snippet)
        {
            throw new NotImplementedException();
            /*if (snippet == null)
            {
                throw new ArgumentNullException(nameof(snippet));
            }
            return new NativeStatement(new CodeSnippetStatement(snippet));*/
        }

        public static ThrowExceptionStatement Throw(Expression toThrow)
        {
            if (toThrow == null)
            {
                throw new ArgumentNullException(nameof(toThrow));
            }
            return new ThrowExceptionStatement(toThrow);
        }

        public static ThrowExceptionStatement Throw(string t, params Expression[] args)
        {
            return Throw(new StringTypeDeclaration(t), args);
        }

        public static ThrowExceptionStatement Throw(Type t, params Expression[] args)
        {
            return Throw(new TypeTypeDeclaration(t), args);
        }

        public static ThrowExceptionStatement Throw(ITypeDeclaration t, params Expression[] args)
        {
            return new ThrowExceptionStatement(Expr.New(t, args));
        }

        public static TryCatchFinallyStatement Try()
        {
            return new TryCatchFinallyStatement();
        }

        public static CatchClause Catch(ParameterDeclaration localParam)
        {
            return new CatchClause(localParam);
        }

        public static CatchClause Catch(string t, string name)
        {
            return Catch(new StringTypeDeclaration(t), name);
        }

        public static CatchClause Catch(Type t, string name)
        {
            return Catch(new TypeTypeDeclaration(t), name);
        }

        public static CatchClause Catch(ITypeDeclaration t, string name)
        {
            return Catch(new ParameterDeclaration(t, name, nonNull: false, FieldDirectionReflyn.In));
        }

        public static TryCatchFinallyStatement Guard(Type expectedExceptionType)
        {
            TryCatchFinallyStatement tryCatchFinallyStatement = Try();
            tryCatchFinallyStatement.CatchClauses.Add(Catch(expectedExceptionType, ""));
            return tryCatchFinallyStatement;
        }
        
        /// <summary>
        /// Declaring a variable inline.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VariableDeclarationStatement Var(string type, string name)
        {
            return Var(new StringTypeDeclaration(type), name);
        }

        public static VariableDeclarationStatement Var(Type type, string name)
        {
            return Var(new TypeTypeDeclaration(type), name);
        }

        public static VariableDeclarationStatement Var(ITypeDeclaration type, string name)
        {
            return new VariableDeclarationStatement(type, name);
        }

        public static VariableDeclarationStatement Var(string type, string name, Expression initExpression)
        {
            return Var(new StringTypeDeclaration(type), name, initExpression);
        }

        public static VariableDeclarationStatement Var(Type type, string name, Expression initExpression)
        {
            return Var(new TypeTypeDeclaration(type), name, initExpression);
        }

        public static VariableDeclarationStatement Var(ITypeDeclaration type, string name, Expression initExpression)
        {
            VariableDeclarationStatement variableDeclarationStatement = Var(type, name);
            variableDeclarationStatement.InitExpression = initExpression;
            return variableDeclarationStatement;
        }

        private static readonly StringTypeDeclaration VarStringTypeDeclaration = new StringTypeDeclaration("var");

        /// <summary>
        /// Variable declaration using the 'var' keyword.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VariableDeclarationStatement VarVar(string name)
        {
            return new VariableDeclarationStatement(VarStringTypeDeclaration, name);
        }

        /// <summary>
        /// Variable declaration using the 'var' keyword.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="initExpression"></param>
        /// <returns></returns>
        public static VariableDeclarationStatement VarVar(string name, Expression initExpression)
        {
            VariableDeclarationStatement variableDeclarationStatement = Var(VarStringTypeDeclaration, name);
            variableDeclarationStatement.InitExpression = initExpression;
            return variableDeclarationStatement;
        }

        // initStatement upgraded to VariableDeclarationStatement
        public static IterationStatement For(VariableDeclarationStatement initStatement, Expression testExpression, StatementWithExpression incrementStatement)
        {
            return new IterationStatement(initStatement, testExpression, incrementStatement);
        }

        public static IterationStatement While(Expression testExpression)
        {
            return new IterationStatement(testExpression);
        }

        public static ForEachStatement ForEach(string localType, string localName, Expression collection, bool enumeratorDisposable)
        {
            return ForEach(new StringTypeDeclaration(localType), localName, collection, enumeratorDisposable);
        }

        public static ForEachStatement ForEach(Type localType, string localName, Expression collection, bool enumeratorDisposable)
        {
            return ForEach(new TypeTypeDeclaration(localType), localName, collection, enumeratorDisposable);
        }

        public static ForEachStatement ForEach(ITypeDeclaration localType, string localName, Expression collection, bool enumeratorDisposable)
        {
            return new ForEachStatement(localType, localName, collection, enumeratorDisposable);
        }

        public static AttachRemoveEventStatement Attach(EventReferenceExpression eventRef, Expression listener)
        {
            return new AttachRemoveEventStatement(eventRef, listener, attach: true);
        }

        public static AttachRemoveEventStatement Remove(EventReferenceExpression eventRef, Expression listener)
        {
            return new AttachRemoveEventStatement(eventRef, listener, attach: false);
        }
    }
}
