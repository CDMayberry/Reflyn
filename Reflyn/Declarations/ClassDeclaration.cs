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
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Declarations
{
    public class ClassDeclaration : Declaration, ClassAccessMixin<ClassDeclaration>, StaticMixin<ClassDeclaration>, ScopeMixin<ClassDeclaration>
    {
        private readonly List<ConstructorDeclaration> _constructors = new List<ConstructorDeclaration>();

        // TODO: Remove and separate Struct / Class / Interface.
        public ClassOutputType OutputType { get; set; } = ClassOutputType.Class;

        public NamespaceDeclaration Namespace { get; }

        public override string FullName => $"{Namespace.FullName}.{base.Name}";

        public ITypeDeclaration Parent { get; set; } = null;

        private SyntaxToken? _partialModifier = null;
        
        public TypeDeclarationList Interfaces { get; } = new TypeDeclarationList();

        //public StringConstantDeclarationDictionary StringConstants { get; } = new StringConstantDeclarationDictionary();

        public StringFieldDeclarationDictionary StringFields { get; } = new StringFieldDeclarationDictionary();

        public List<EventDeclaration> Events { get; } = new List<EventDeclaration>();

        public PropertyDeclarationList Properties { get; } = new PropertyDeclarationList();

        public MethodDeclarationList Methods { get; } = new MethodDeclarationList();

        public DelegateDeclarationList Delegates { get; } = new DelegateDeclarationList();

        public StringClassDeclarationDictionary NestedStringClasses { get; } = new StringClassDeclarationDictionary();

        public List<IndexerDeclaration> Indexers { get; } = new List<IndexerDeclaration>();

        public StringSet Imports { get; } = new StringSet();

        public string InterfaceName => $"I{base.Name}";

        public ClassDeclaration(string name, NamespaceDeclaration ns)
            : base(name, ns.Conformer)
        {
            this.Namespace = ns;
            this.ToPublic();
        }

        // Type Modifiers
        public ClassDeclaration ToClass()
        {
            OutputType = ClassOutputType.Class;
            return this;
        }

        public ClassDeclaration ToStruct()
        {
            OutputType = ClassOutputType.Struct;
            return this;
        }

        public ClassDeclaration ToInterface()
        {
            OutputType = ClassOutputType.Interface;
            return this;
        }
        
        public ClassDeclaration ToPartial()
        {
            _partialModifier = Token(SyntaxKind.PartialKeyword);
            return this;
        }


        public override string[] GetImports()
        {
            var imports = new List<string>(base.GetImports());

            if (Parent != null)
            {
               imports.AddRange(Parent.GetImports());
            }

            if (CustomAttributes.Count > 0)
            {
                imports.AddRange(_customAttributes.GetImports());
            }

            if (Interfaces.Count > 0)
            {
                imports.AddRange(Interfaces.GetImports());
            }

            if (Properties.Count > 0)
            {
                imports.AddRange(Properties.GetImports());
            }

            if (StringFields.Count > 0)
            {
                imports.AddRange(StringFields.GetImports());
            }

            if (Methods.Count > 0)
            {
                imports.AddRange(Methods.GetImports());
            }

            return imports.ToArray();
        }

        public ClassDeclaration SetBaseType<T>()
        {
            return SetBaseType(typeof(T));
        }

        public ClassDeclaration SetBaseType(Type type)
        {
            Parent = new TypeTypeDeclaration(type);
            return this;
        }

        public ClassDeclaration SetBaseType(string type)
        {
            Parent = new StringTypeDeclaration(type);
            return this;
        }

        public ClassDeclaration AddInterface<T>()
        {
            return AddInterface(typeof(T));
        }

        public ClassDeclaration AddInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new Exception(type + " is not an interface.");
            }

            Interfaces.Add(type);
            return this;
        }

        /// <summary>
        /// Unsafe usage as this does not verify it if is actually a interface, prefer to use the version with the Type parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ClassDeclaration AddInterface(ITypeDeclaration type)
        {
            Interfaces.Add(type);
            return this;
        }

        /// <summary>
        /// Unsafe usage as this does not verify it if is actually a interface, prefer to use the version with the Type parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ClassDeclaration AddInterface(string type)
        {
            Interfaces.Add(new StringTypeDeclaration(type));
            return this;
        }

        public FieldDeclaration AddConstant(Type type, string name, Expression assignment)
        {
            if (assignment == null)
            {
                throw new NullReferenceException("assignment cannot be null, a constant requires a value.");
            }

            var field = AddField(type, name);
            field.ToConst();
            field.InitExpression = assignment;
            return field;
        }

        public FieldDeclaration[] Fields => StringFields.Values.ToArray();

        public FieldDeclaration AddField(ITypeDeclaration type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (StringFields.ContainsKey(name))
            {
                throw new ArgumentException("field already existing in class: " + name);
            }
            var fieldDeclaration = new FieldDeclaration(name, this, type); // base.Conformer.ToPascal(name) replaced with name, I think this should be left up to the caller and not done here.
            StringFields.Add(fieldDeclaration);
            return fieldDeclaration;
        }

        public FieldDeclaration AddField(Type type, string name)
        {
            return AddField(new TypeTypeDeclaration(type), name);
        }

        public FieldDeclaration AddField(string type, string name)
        {
            return AddField(new StringTypeDeclaration(type), name);
        }

        public FieldGenericDeclaration AddField<T>(Type type, string name)
        {
            return AddGenericField<T>(new TypeTypeDeclaration(type), name);
        }

        public FieldGenericDeclaration AddField<T>(string type, string name)
        {
            return AddGenericField<T>(new StringTypeDeclaration(type), name);
        }

        public FieldGenericDeclaration AddField<T, T2>(Type type, string name)
        {
            return AddGenericField<T, T2>(new TypeTypeDeclaration(type), name);
        }

        public FieldGenericDeclaration AddField<T, T2>(string type, string name)
        {
            return AddGenericField<T, T2>(new StringTypeDeclaration(type), name);
        }

        public FieldGenericDeclaration AddGenericField<T>(ITypeDeclaration type, string name)
        {
            return AddGenericField(type, name, new TypeTypeDeclaration(typeof(T)));
        }

        public FieldGenericDeclaration AddGenericField<T, T2>(ITypeDeclaration type, string name)
        {
            return AddGenericField(type, name, new TypeTypeDeclaration(typeof(T)), new TypeTypeDeclaration(typeof(T2)));
        }

        public FieldGenericDeclaration AddGenericField(ITypeDeclaration type, string name, params string[] genericParams)
        {
            return AddGenericField(type, name, genericParams.Select(x => (ITypeDeclaration)new StringTypeDeclaration(x)).ToArray());
        }

        public FieldGenericDeclaration AddGenericField(string type, string name, params string[] genericParams)
        {
            return AddGenericField(new StringTypeDeclaration(type), name, genericParams.Select(x => (ITypeDeclaration)new StringTypeDeclaration(x)).ToArray());
        }

        public FieldGenericDeclaration AddGenericField(ITypeDeclaration type, string name, params Type[] genericParams)
        {
            return AddGenericField(type, name, genericParams.Select(x => (ITypeDeclaration)new TypeTypeDeclaration(x)).ToArray());
        }

        public FieldGenericDeclaration AddGenericField(ITypeDeclaration type, string name, params ITypeDeclaration[] genericParams)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (StringFields.ContainsKey(name))
            {
                throw new ArgumentException("field already existing in class: " + name);
            }
            var fieldDeclaration = new FieldGenericDeclaration(name, this, type, genericParams); // base.Conformer.ToPascal(name) replaced with name, I think this should be left up to the caller and not done here.
            StringFields.Add(fieldDeclaration);
            return fieldDeclaration;
        }

        public FieldArrayDeclaration AddFieldArray(ITypeDeclaration type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (StringFields.ContainsKey(name))
            {
                throw new ArgumentException("field already existing in class: " + name);
            }
            var fieldDeclaration = new FieldArrayDeclaration(name, this, type); // base.Conformer.ToPascal(name) replaced with name, I think this should be left up to the caller and not done here.
            StringFields.Add(fieldDeclaration);
            return fieldDeclaration;
        }

        public FieldArrayDeclaration AddFieldArray(Type type, string name)
        {
            return AddFieldArray(new TypeTypeDeclaration(type), name);
        }

        public FieldArrayDeclaration AddFieldArray(string type, string name)
        {
            return AddFieldArray(new StringTypeDeclaration(type), name);
        }

        public EventDeclaration AddEvent(ITypeDeclaration type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var eventDeclaration = new EventDeclaration(base.Conformer.ToPascal(name), this, type);
            Events.Add(eventDeclaration);
            return eventDeclaration;
        }

        public EventDeclaration AddEvent(Type type, string name)
        {
            return AddEvent(new TypeTypeDeclaration(type), name);
        }

        public EventDeclaration AddEvent(string type, string name)
        {
            return AddEvent(new StringTypeDeclaration(type), name);
        }

        public ClassDeclaration AddClass(string name)
        {
            var classDeclaration = new ClassDeclaration(base.Conformer.ToPascal(name), Namespace);
            NestedStringClasses.Add(classDeclaration);
            return classDeclaration;
        }

        public IndexerDeclaration AddIndexer(ITypeDeclaration type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            var indexerDeclaration = new IndexerDeclaration(this, type);
            Indexers.Add(indexerDeclaration);
            return indexerDeclaration;
        }

        public IndexerDeclaration AddIndexer(Type type)
        {
            return AddIndexer(new TypeTypeDeclaration(type));
        }

        public IndexerDeclaration AddIndexer(string type)
        {
            return AddIndexer(new StringTypeDeclaration(type));
        }

        public PropertyDeclaration AddProperty(ITypeDeclaration type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var propertyDeclaration = new PropertyDeclaration(name, this, type);// base.Conformer.ToPascal(name) replaced with name, I think this should be left up to the caller and not done here.
            Properties.Add(propertyDeclaration);
            return propertyDeclaration;
        }

        // TODO: Create WithProperty Methods that return the ClassDeclaration, in the case where the developer doesn't need additional changes.
        public PropertyDeclaration AddProperty(Type type, string name)
        {
            return AddProperty(new TypeTypeDeclaration(type), name);
        }

        public PropertyDeclaration AddProperty(string type, string name)
        {
            return AddProperty(new StringTypeDeclaration(type), name);
        }

        public PropertyDeclaration AddProperty(FieldDeclaration f, string name, bool hasGet, bool hasSet, bool checkNonNull)
        {
            PropertyDeclaration propertyDeclaration = AddProperty(f.Type, name);
            if (hasGet)
            {
                propertyDeclaration.Get.Return(Expr.This.Field(f));
            }
            if (hasSet)
            {
                if (checkNonNull)
                {
                    ConditionStatement conditionStatement = Stm.If(Expr.Value.Identity(Expr.Null));
                    propertyDeclaration.Set.Add(conditionStatement);
                    conditionStatement.TrueStatements.Add(Stm.Throw(typeof(ArgumentNullException)));
                    propertyDeclaration.SetExceptions.Add(new ThrowedExceptionDeclaration(typeof(ArgumentNullException), "value is a null reference"));
                }
                propertyDeclaration.Set.Add(Stm.Assign(Expr.This.Field(f), Expr.Value));
            }
            return propertyDeclaration;
        }

        public PropertyDeclaration AddProperty(FieldDeclaration f, bool hasGet, bool hasSet, bool checkNonNull)
        {
            return AddProperty(f, f.Name, hasGet, hasSet, checkNonNull);
        }
        
        public PropertyDeclaration AddAutoProperty(Type type, string name = null)
        {
            return AddProperty(type, name ?? type.Name).ToAutoProperty();

        }

        public PropertyDeclaration AddAutoProperty(string type, string name)
        {
            return AddProperty(type, name).ToAutoProperty();
        }

        public ConstructorDeclaration AddConstructor()
        {
            var constructorDeclaration = new ConstructorDeclaration(this);
            _constructors.Add(constructorDeclaration);
            return constructorDeclaration;
        }

        public MethodDeclaration AddMethod(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var methodDeclaration = new MethodDeclaration(name, this);
            Methods.Add(methodDeclaration);
            return methodDeclaration;
        }

        public DelegateDeclaration AddDelegate(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var delegateDeclaration = new DelegateDeclaration(name, this);
            Delegates.Add(delegateDeclaration);
            return delegateDeclaration;
        }
        
        private BaseListSyntax GetBaseList()
        {
            // WithBaseList can accept a null argument so this is valid.
            if (Parent == null && Interfaces.Count <= 0)
            {
                return null;
            }

            SeparatedSyntaxList<BaseTypeSyntax> modifierTokens;

            // I have a headache, we're just doing this the dumb way.
            if (Parent != null)
            {
                var nodeOrToken = new SyntaxNodeOrToken[]
                {
                    SimpleBaseType(
                        IdentifierName(Parent.Name)
                    )
                };


                foreach (var interfaze in Interfaces)
                {
                    nodeOrToken = nodeOrToken
                        .Concat(new SyntaxNodeOrToken[]
                            {
                                Token(SyntaxKind.CommaToken),
                                SimpleBaseType(
                                    IdentifierName(interfaze.Name)
                                )
                            }
                        )
                        .ToArray();
                }

                modifierTokens = SeparatedList<BaseTypeSyntax>(nodeOrToken);
            }
            else
            {
                var nodeOrToken = new SyntaxNodeOrToken[]
                {
                    SimpleBaseType(
                        IdentifierName(Interfaces[0].Name)
                    )
                };

                foreach (var interfaze in Interfaces.Skip(1))
                {
                    nodeOrToken = nodeOrToken
                        .Concat(new SyntaxNodeOrToken[]
                            {
                                Token(SyntaxKind.CommaToken),
                                SimpleBaseType(
                                    IdentifierName(interfaze.Name)
                                )
                            }
                        )
                        .ToArray();
                }

                modifierTokens = SeparatedList<BaseTypeSyntax>(nodeOrToken);
            }

            return BaseList(
                modifierTokens
            );
        }

        private TypeDeclarationSyntax GetOutputType()
        {
            TypeDeclarationSyntax type;
            if (OutputType == ClassOutputType.Class)
            {
                type = ClassDeclaration(Name);
            }
            else if (OutputType == ClassOutputType.Struct)
            {
                type = StructDeclaration(Name);
            }
            else
            {
                type = InterfaceDeclaration(Name);
            }

            return type.WithBaseList(GetBaseList());
        }


        public override MemberDeclarationSyntax ToSyntax()
        {
            TypeDeclarationSyntax syntax = 
                GetOutputType()
                    .WithModifiers(
                        GetModifierTokens(ScopeModifier, StaticModifier, AccessModifier)
                    )
                    .WithAttributeLists(
                        GetCustomAttributes()
                    );

            // 'classes' can be structs here.
            if (this.Delegates.Count > 0)
            {
                syntax = syntax.AddMembers(
                    Delegates.Select(x => x.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);

            }

            if (this.Events.Count > 0)
            {
                syntax = syntax.AddMembers(
                    Events.Select(x => x.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);

            }

            if (this.NestedStringClasses.Count > 0)
            {
                syntax = syntax.AddMembers(
                    NestedStringClasses.Select(x => x.Value.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);

            }

            if (this.StringFields.Count > 0)
            {
                syntax = syntax
                    .AddMembers(
                        Fields.Select(x => x.ToSyntax()).ToArray()
                    )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);
            }

            if (this.Properties.Count > 0)
            {
                syntax = syntax
                    .AddMembers(
                    Properties.Select(x => x.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);
            }

            if (this._constructors.Count > 0)
            {
                syntax = syntax.AddMembers(
                    _constructors.Select(x => x.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);
            }

            if (this.Methods.Count > 0)
            {
                syntax = syntax.AddMembers(
                    Methods.Select(x => x.ToSyntax()).ToArray()
                )
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturn);
            }

            return syntax;
        }

        public SyntaxToken? AccessModifier { get; set; }
        public SyntaxToken? StaticModifier { get; set; }
        public SyntaxToken? ScopeModifier { get; set; }
    }
}
