using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Extensions;
using Reflyn.Mixins;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class EnumDeclaration : MemberDeclaration, ScopeMixin<EnumDeclaration>
    {
        private bool _flags;
        public ITypeDeclaration BaseType { get; set; } = null;

        // This has way too many nested generics.
        public List<EnumMemberDeclaration> Fields { get; } = new List<EnumMemberDeclaration>();

        internal EnumDeclaration(string name, Declaration declaringType, bool flags) : base(name, declaringType)
        {
            this._flags = flags;
            this.ToPublic();
        }

        public EnumMemberDeclaration AddField(string name)
        {
            var fieldDeclaration = new EnumMemberDeclaration(name, this);
            Fields.Add(fieldDeclaration);
            return fieldDeclaration;
        }

        public EnumMemberDeclaration AddField(string name, int value)
        {
            EnumMemberDeclaration fieldDeclaration = AddField(name);
            fieldDeclaration.EqualTo(Expr.Prim(value));
            return fieldDeclaration;
        }

        // Unmanaged is a rough limiter, enums can only be discrete value types, but this technically allows floats which is wrong. But it's better than allowing any Type.
        public EnumDeclaration SetBaseType<TK>() where TK : unmanaged
        {
            var type = typeof(TK);
            if (type != typeof(long) && type != typeof(int) && type != typeof(short) && type != typeof(byte)
                && type != typeof(ulong) && type != typeof(uint) && type != typeof(ushort))
            {
                throw new Exception("Enum can only use discrete value types but was " + type.Name);
            }


            return SetBaseType(typeof(TK));
        }

        private EnumDeclaration SetBaseType(Type type)
        {
            BaseType = new TypeTypeDeclaration(type);
            return this;
        }

        public override MemberDeclarationSyntax ToSyntax()
        {
            var result = EnumDeclaration(Name)
                .WithModifiers(
                    GetModifierTokens(ScopeModifier)
                )
                .WithAttributeLists(GetCustomAttributes());

            if (BaseType != null)
            {
                result = result.WithBaseList(
                    BaseList(
                        SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                BaseType.ToTypeSyntax()
                            )
                        )
                    )
                );
            }

            SyntaxNodeOrTokenList list = Fields.ToSeparatedTokenList();
            result = result
                .WithMembers(
                    SeparatedList<EnumMemberDeclarationSyntax>(list)
                );

            return result;
        }

        public SyntaxToken? ScopeModifier { get; set; }
    }
}
