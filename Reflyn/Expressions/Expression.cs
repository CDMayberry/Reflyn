using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Utilities; //using System.CodeDom;

namespace Reflyn.Expressions
{
    public abstract class Expression
    {
        public virtual string[] GetImports()
        {
            return new string[] { };
        }

        public abstract ExpressionSyntax ToSyntax();
        //public abstract CodeExpression ToCodeDom();

        public FieldReferenceExpression Field(FieldDeclaration field)
        {
            return new FieldReferenceExpression(this, field);
        }

        public NativePropertyReferenceExpression Field(string prop)
        {
            return new NativePropertyReferenceExpression(this, prop);
        }

        public PropertyReferenceExpression Prop(PropertyDeclaration prop)
        {
            return new PropertyReferenceExpression(this, prop);
        }

        public NativePropertyReferenceExpression Prop(string prop)
        {
            return new NativePropertyReferenceExpression(this, prop);
        }

        public MethodReferenceExpression Method(MethodDeclaration method)
        {
            return MethodReferenceExpression.From(this, method);
        }

        // Native likely means just use a code snippet.
        public MethodReferenceExpression Method(string method)
        {
            return new MethodReferenceExpression(this, method);
        }

        // My custom code... With Roslyn can we make a Generic friendly method?
        public MethodReferenceExpression GenericMethod<T>(string method)
        {
            return GenericMethod(method, typeof(T));
        }

        // My custom code... With Roslyn can we make a Generic friendly method?
        public MethodReferenceExpression GenericMethod(string method, Type type)
        {
            return new MethodReferenceExpression(this, method, Expr.Type(type));
        }
        public MethodReferenceExpression GenericMethod(string method, string type)
        {
            
            return new MethodReferenceExpression(this, method, Expr.Type(type));
        }

        public MethodReferenceExpression Event(string name)
        {
            return new MethodReferenceExpression(this, name);
        }

        public MethodReferenceExpression Event(EventDeclaration e)
        {
            return MethodReferenceExpression.From(this, e);
        }

        public CastExpression Cast(string t)
        {
            return Cast(new StringTypeDeclaration(t));
        }

        public CastExpression Cast(Type t)
        {
            return Cast(new TypeTypeDeclaration(t));
        }

        public CastExpression Cast(ITypeDeclaration t)
        {
            return new CastExpression(t, this);
        }

        // TODO: Do we want to move most of these to PrimitiveExpression? These are largely math operations...

        // BUG: These don't work. I think ++ and -- expect the class to return it's original type and not a new type.
        public static UnaryOperatorExpression operator ++(Expression expr)
        {
            return new UnaryOperatorExpression(expr, true, true);
        }
        public static UnaryOperatorExpression operator --(Expression expr)
        {
            return new UnaryOperatorExpression(expr, false, true);
        }

        public static BinaryOpOperatorExpression operator +(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Add);
        }

        public static BinaryOpOperatorExpression operator -(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Subtract);
        }

        public static BinaryOpOperatorExpression operator /(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Divide);
        }

        // Don't think this works, I think the first argument kinda has to be an expression.
        public static BinaryOpOperatorExpression operator /(int left, Expression right)
        {
            return new BinaryOpOperatorExpression(Expr.Prim(left), right, CodeBinaryOperatorTypeReflyn.Divide);
        }
        
        public static BinaryOpOperatorExpression operator *(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Multiply);
        }
        
        public static BinaryOpOperatorExpression operator %(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.Modulus);
        }
        
        public static BinaryOpOperatorExpression operator <(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThan);
        }
        
        public static BinaryOpOperatorExpression operator <=(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.LessThanOrEqual);
        }
        
        public static BinaryOpOperatorExpression operator >(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThan);
        }

        public static BinaryOpOperatorExpression operator >=(Expression left, Expression right)
        {
            return new BinaryOpOperatorExpression(left, right, CodeBinaryOperatorTypeReflyn.GreaterThanOrEqual);
        }

        public BinaryOpOperatorExpression Identity(Expression right)
        {
            return new BinaryOpOperatorExpression(this, right, CodeBinaryOperatorTypeReflyn.IdentityEquality);
        }

        public BinaryOpOperatorExpression NotIdentity(Expression right)
        {
            return new BinaryOpOperatorExpression(this, right, CodeBinaryOperatorTypeReflyn.IdentityInequality);
        }

        public BinaryOpOperatorExpression And(Expression right)
        {
            return new BinaryOpOperatorExpression(this, right, CodeBinaryOperatorTypeReflyn.BooleanAnd);
        }

        public BinaryOpOperatorExpression Or(Expression right)
        {
            return new BinaryOpOperatorExpression(this, right, CodeBinaryOperatorTypeReflyn.BooleanOr);
        }

        // TODO: Should these be moved to VariableReferenceExpressions? Putting them on base seems like it'd lead to headaches and overcomplicates the base class.
        public IndexerExpression Item(int index)
        {
            return Item(Expr.Prim(index));
        }

        public IndexerExpression Item(params Expression[] indices)
        {
            return new IndexerExpression(this, indices);
        }

        public ArrayIndexerExpression ArrayItem(int index)
        {
            return new ArrayIndexerExpression(this, Expr.Prim(index));
        }

        public ArrayIndexerExpression ArrayItem(Expression index)
        {
            return new ArrayIndexerExpression(this, index);
        }


        // Avoiding string atm, just value types.
        public static implicit operator Expression(bool val) => Expr.Prim(val);
        public static implicit operator Expression(short val) => Expr.Prim(val);
        public static implicit operator Expression(int val) => Expr.Prim(val);
        public static implicit operator Expression(long val) => Expr.Prim(val);
        public static implicit operator Expression(float val) => Expr.Prim(val);
        public static implicit operator Expression(double val) => Expr.Prim(val);
    }
}
