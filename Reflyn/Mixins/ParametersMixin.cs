using System;
using Reflyn.Declarations;
using Reflyn.Utilities;

namespace Reflyn.Mixins
{
    public interface ParametersMixin<T> where T : Declaration
    {
        MethodSignature Signature { get; }
    }

    public static class ParametersMixinExtensions
    {
        // WithParameter<uint> doesn't work because it defaults to the other WithParameter methods without a explicit second generic type.
        /*public static T WithParameter<TK, T>(this ParametersMixin<T> mixin, string name) where T : Declaration
        {
            return mixin.WithParameter(typeof(TK), name);
        }*/

        public static T WithParameter<T>(this ParametersMixin<T> mixin, ParameterDeclaration param) where T : Declaration
        {
            // TODO: Call AddParameter here instead.
            mixin.Signature.Parameters.Add(param);
            return (T)mixin;
        }

        public static T WithParameter<T>(this ParametersMixin<T> mixin, ITypeDeclaration type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            mixin.AddParameter(type, name, direction);
            return (T)mixin;
        }

        public static T WithParameter<T>(this ParametersMixin<T> mixin, Type type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            mixin.AddParameter(type, name, direction);
            return (T)mixin;
        }

        public static T WithParameter<T>(this ParametersMixin<T> mixin, string type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            mixin.AddParameter(type, name, direction);
            return (T)mixin;
        }

        public static ParameterDeclaration AddParameter<TK, T>(this ParametersMixin<T> mixin, string name) where T : Declaration
        {
            return mixin.AddParameter(typeof(TK), name);
        }

        public static ParameterDeclaration AddParameter<T>(this ParametersMixin<T> mixin, ITypeDeclaration type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            return mixin.Signature.Parameters.Add(type, name, direction: direction);
        }

        public static ParameterDeclaration AddParameter<T>(this ParametersMixin<T> mixin, Type type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            return mixin.Signature.Parameters.Add(type, name, direction: direction);
        }

        public static ParameterDeclaration AddParameter<T>(this ParametersMixin<T> mixin, string type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In) where T : Declaration
        {
            return mixin.Signature.Parameters.Add(type, name, direction: direction);
        }

        /*public MethodDeclaration<T> WithParameter<TK>(string name)
        {
            return WithParameter(typeof(TK), name);
        }

        public MethodDeclaration<T> WithParameter(ParameterDeclaration param)
        {
            Signature.Parameters.Add(param);
            return this;
        }

        public MethodDeclaration<T> WithParameter(ITypeDeclaration type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            Signature.Parameters.Add(type, name, direction: direction);
            return this;
        }

        public MethodDeclaration<T> WithParameter(Type type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            Signature.Parameters.Add(type, name, direction: direction);
            return this;
        }

        public MethodDeclaration<T> WithParameter(string type, string name, FieldDirectionReflyn direction = FieldDirectionReflyn.In)
        {
            Signature.Parameters.Add(type, name, direction: direction);
            return this;
        }

        public ParameterDeclaration AddParameter<TK>(string name)
        {
            return AddParameter(typeof(TK), name);
        }

        public ParameterDeclaration AddParameter(ITypeDeclaration type, string name)
        {
            return Signature.Parameters.Add(type, name);
        }

        public ParameterDeclaration AddParameter(Type type, string name)
        {
            return Signature.Parameters.Add(type, name);
        }

        public ParameterDeclaration AddParameter(string type, string name)
        {
            return Signature.Parameters.Add(type, name);
        }
        */
    }
}
