using System;
using System.Linq;
using Reflyn.Expressions;
using Reflyn.Statements;
using Reflyn.Utilities;

namespace Reflyn.Declarations
{
    public static class Expr
    {
        public static ThisReferenceExpression This => new ThisReferenceExpression();

        public static BaseReferenceExpression Base => new BaseReferenceExpression();

        /// <summary>
        /// This is the literal keyword `value`
        /// </summary>
        public static PropertySetValueReferenceExpression Value => new PropertySetValueReferenceExpression();

        public static PrimitiveNullExpression Null => new PrimitiveNullExpression();

        public static PrimitiveExpression<bool> True => Prim(true);

        public static PrimitiveExpression<bool> False => Prim(false);

        public static class Keywords
        {
            public static ThisReferenceExpression This => new ThisReferenceExpression();

            public static BaseReferenceExpression Base => new BaseReferenceExpression();

            /// <summary>
            /// This is the literal keyword `value`
            /// </summary>
            public static PropertySetValueReferenceExpression Value => new PropertySetValueReferenceExpression();

            public static PrimitiveNullExpression Null => new PrimitiveNullExpression();

            public static PrimitiveExpression<bool> True => Prim(true);

            public static PrimitiveExpression<bool> False => Prim(false);
        }
        
        public static AwaitExpression Await(Expression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new AwaitExpression(expr);
        }

        public static ParenthesisExpression Parens(Expression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new ParenthesisExpression(expr);
        }

        public static ObjectCreationExpression New(string type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return New(new StringTypeDeclaration(type), parameters);
        }

        public static ObjectCreationExpression New(Type type, params Expression[] parameters)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return New(new TypeTypeDeclaration(type), parameters);
        }

        public static ObjectCreationExpression New(ITypeDeclaration type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ObjectCreationExpression(type, parameters);
        }

        public static ObjectCreationExpression New<T>(string type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return NewGeneric<T>(new StringTypeDeclaration(type),  parameters);
        }

        public static ObjectCreationExpression New<T>(Type type, params Expression[] parameters)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return NewGeneric<T>(new TypeTypeDeclaration(type),  parameters);
        }

        public static ObjectCreationExpression New<T, T2>(string type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return NewGeneric<T, T2>(new StringTypeDeclaration(type), parameters);
        }

        public static ObjectCreationExpression New<T, T2>(Type type, params Expression[] parameters)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return NewGeneric<T, T2>(new TypeTypeDeclaration(type), parameters);
        }

        // TODO: Find a way to compress all of these different variations
        public static ObjectGenericCreationExpression NewGeneric(ITypeDeclaration type, string genericType, params Expression[] parameters)
        {
            return NewGeneric(type, new ITypeDeclaration[] { new StringTypeDeclaration(genericType) }, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(ITypeDeclaration type, string[] genericTypes, params Expression[] parameters)
        {
            return NewGeneric(type, genericTypes.Select(x => (ITypeDeclaration) new StringTypeDeclaration(x)).ToArray(), parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(ITypeDeclaration type, ITypeDeclaration genericType, params Expression[] parameters)
        {
            return NewGeneric(type, new [] { genericType } , parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(string type, ITypeDeclaration genericType, params Expression[] parameters)
        {
            return NewGeneric(new StringTypeDeclaration(type), new [] { genericType }, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(string type, ITypeDeclaration[] genericTypes, params Expression[] parameters)
        {
            return NewGeneric(new StringTypeDeclaration(type), genericTypes, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(string type, string[] genericTypes, params Expression[] parameters)
        {
            return NewGeneric(new StringTypeDeclaration(type), genericTypes.Select(x => (ITypeDeclaration)new StringTypeDeclaration(x)).ToArray(), parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(string type, params Expression[] parameters)
        {
            return NewGeneric(new StringTypeDeclaration(type), parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(ITypeDeclaration type, ITypeDeclaration[] genericTypes, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ObjectGenericCreationExpression(type, genericTypes, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric(ITypeDeclaration type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ObjectGenericCreationExpression(type, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric<T>(ITypeDeclaration type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ObjectGenericCreationExpression(type, new ITypeDeclaration[] { new TypeTypeDeclaration(typeof(T)) }, parameters);
        }

        public static ObjectGenericCreationExpression NewGeneric<T, T2>(ITypeDeclaration type, params Expression[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ObjectGenericCreationExpression(type, new ITypeDeclaration[] { new TypeTypeDeclaration(typeof(T)), new TypeTypeDeclaration(typeof(T2)) }, parameters);
        }

        public static ArrayCreationWithSizeExpression NewArray(Type type, int size)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithSizeExpression(new TypeTypeDeclaration(type), Prim(size));
        }

        public static ArrayCreationWithSizeExpression NewArray(string type, int size)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithSizeExpression(new StringTypeDeclaration(type), Prim(size));
        }

        public static ArrayCreationWithSizeExpression NewArray(ITypeDeclaration type, int size)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithSizeExpression(type, Prim(size));
        }

        public static ArrayCreationWithSizeExpression NewArray(Type type, Expression sizeExpression)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithSizeExpression(new TypeTypeDeclaration(type), sizeExpression);
        }

        public static ArrayCreationWithSizeExpression NewArray(string type, Expression sizeExpression)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithSizeExpression(new StringTypeDeclaration(type), sizeExpression);
        }

        public static ArrayCreationWithSizeExpression NewArray(ITypeDeclaration type, Expression sizeExpression)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (sizeExpression == null)
            {
                throw new ArgumentNullException(nameof(sizeExpression));
            }
            return new ArrayCreationWithSizeExpression(type, sizeExpression);
        }

        public static ArrayCreationWithInitializersExpression NewArray(Type type, params Expression[] initialiers)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithInitializersExpression(new TypeTypeDeclaration(type), initialiers);
        }

        public static ArrayCreationWithInitializersExpression NewArray(string type, params Expression[] initialiers)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithInitializersExpression(new StringTypeDeclaration(type), initialiers);
        }

        public static ArrayCreationWithInitializersExpression NewArray(ITypeDeclaration type, params Expression[] initialiers)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new ArrayCreationWithInitializersExpression(type, initialiers);
        }

        public static ParameterReferenceExpression Arg(ParameterDeclaration p)
        {
            if (p == null)
            {
                throw new ArgumentNullException(nameof(p));
            }
            return new ParameterReferenceExpression(p);
        }

        public static ParameterReferenceExpression Arg(Declaration dec, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            if (dec == null)
            {
                throw new ArgumentNullException(nameof(dec));
            }
            return new ParameterReferenceExpression(dec, direction);
        }

        public static ParameterReferenceExpression Arg(VariableReferenceExpression variable, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            return new ParameterReferenceExpression(variable, direction);
        }

        public static ParameterReferenceExpression Arg(Expression expr, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            if (expr == null)
            {
                throw new ArgumentNullException(nameof(expr));
            }
            return new ParameterReferenceExpression(expr, direction);
        }

        // TODO: We need a generic declaration version
        public static ParameterReferenceExpression Arg(string variable, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            return new ParameterReferenceExpression(variable, direction);
        }

        public static VariableReferenceExpression Var(VariableDeclarationStatement variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            return new VariableReferenceExpression(variable);
        }

        public static VariableReferenceExpression Var(string variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            return new VariableReferenceExpression(variable);
        }

        public static PrimitiveExpression<T> Prim<T>(T value) where T : unmanaged
        {
            return new PrimitiveExpression<T>(value);
        }

        public static PrimitiveBooleanExpression Prim(bool value)
        {
            return new PrimitiveBooleanExpression(value);
        }

        public static PrimitiveStringExpression Prim(string value)
        {
            return new PrimitiveStringExpression(value);
        }

        public static CastExpression Cast(string type, Expression expr)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return Cast(new StringTypeDeclaration(type), expr);
        }

        public static CastExpression Cast(Type type, Expression expr)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return Cast(new TypeTypeDeclaration(type), expr);
        }

        public static CastExpression Cast(ITypeDeclaration type, Expression expr)
        {
            return new CastExpression(type, expr);
        }

        public static TypeOfExpression TypeOf(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return TypeOf(new StringTypeDeclaration(type));
        }

        public static TypeOfExpression TypeOf<T>()
        {
            return TypeOf(new TypeTypeDeclaration(typeof(T)));
        }

        public static TypeOfExpression TypeOf(Type type)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return TypeOf(new TypeTypeDeclaration(type));
        }

        public static TypeOfExpression TypeOf(ITypeDeclaration type)
        {
            return new TypeOfExpression(type);
        }

        /// <summary>
        /// Roslyn will attempt to parse the input. Use at your own risk.
        /// </summary>
        /// <param name="snippet">A block of C# code, in the form of an expression.</param>
        /// <returns></returns>
        public static SnippetExpression Snippet(string snippet)
        {
            return new SnippetExpression(snippet);
        }

        public static TypeReferenceExpression Type(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return Type(new StringTypeDeclaration(type));
        }

        public static TypeReferenceExpression Type(Type type)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return Type(new TypeTypeDeclaration(type));
        }

        public static TypeReferenceExpression Type(ITypeDeclaration type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return new TypeReferenceExpression(type);
        }


        // Saving Delegates/Events/etc. for last
        public static DelegateCreateExpression Delegate(ITypeDeclaration delegateType, MethodReferenceExpression method)
        {
            if (delegateType == null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            return new DelegateCreateExpression(delegateType, method);
        }

        public static DelegateCreateExpression Delegate(string delegateType, MethodReferenceExpression method)
        {
            if (delegateType == null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            return new DelegateCreateExpression(new StringTypeDeclaration(delegateType), method);
        }

        public static DelegateCreateExpression Delegate(Type delegateType, MethodReferenceExpression method)
        {
            if ((object)delegateType == null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            return new DelegateCreateExpression(new TypeTypeDeclaration(delegateType), method);
        }

        public static DelegateInvokeExpression DelegateInvoke<T>(EventReferenceExpression firedEvent, params Expression[] parameters)
        {
            if (firedEvent == null)
            {
                throw new ArgumentNullException(nameof(firedEvent));
            }
            return new DelegateInvokeExpression(firedEvent, parameters);
        }

        public static LambdaExpression Lambda(params string[] parameters)
        {
            var lambda = new LambdaExpression();
            foreach (var param in parameters)
            {
                lambda.AddArg(param);
            }

            return lambda;
        }
    }
}
