/*
using Refly.CodeDom;
using Refly.CodeDom.Statements;
using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;

namespace Refly.Templates
{
	public class CollectionTemplate : Template
	{
		[Category("Data")]
		public string ElementType = null;

		[Category("Data")]
		public bool ItemGet = true;

		[Category("Data")]
		public bool ItemSet = true;

		[Category("Data")]
		public bool Add = true;

		[Category("Data")]
		public bool AddRange = true;

		[Category("Data")]
		public bool Contains = true;

		[Category("Data")]
		public bool Remove = true;

		[Category("Data")]
		public bool Insert = true;

		[Category("Data")]
		public bool IndexOf = true;

		[Category("Enumerator")]
		public bool Enumerator = true;

		public ITypeDeclaration Type
		{
			get
			{
				if (ElementType == null)
				{
					throw new InvalidOperationException("Element type is undefined");
				}
				return new StringTypeDeclaration(ElementType);
			}
		}

		public string CollectionName => string.Format(base.NameFormat, Type.Name);

		public CollectionTemplate()
			: base("Strongly Typed Collections", "{0}Collection")
		{
		}

		public ClassDeclaration AddClass(NamespaceDeclaration ns)
		{
			ClassDeclaration classDeclaration = ns.AddClass(CollectionName);
			classDeclaration.Parent = new TypeTypeDeclaration(typeof(CollectionBase));
			classDeclaration.AddConstructor();
			if (ItemGet || ItemSet)
			{
				IndexerDeclaration indexerDeclaration = classDeclaration.AddIndexer(Type);
				ParameterDeclaration p = indexerDeclaration.Signature.Parameters.Add(typeof(int), "index", nonNull: false);
				if (ItemGet)
				{
					indexerDeclaration.Get.Return(Expr.This.Prop("List").Item(Expr.Arg(p)).Cast(Type));
				}
				if (ItemSet)
				{
					indexerDeclaration.Set.Add(Stm.Assign(Expr.This.Prop("List").Item(Expr.Arg(p)), Expr.Value));
				}
			}
			string name = ns.Conformer.ToCamel(Type.Name);
			if (Add)
			{
				MethodDeclaration methodDeclaration = classDeclaration.AddMethod("Add");
				ParameterDeclaration parameterDeclaration = methodDeclaration.Signature.Parameters.Add(Type, name, nonNull: true);
				methodDeclaration.Body.Add(Expr.This.Prop("List").Method("Add").Invoke(parameterDeclaration));
			}
			if (AddRange)
			{
				MethodDeclaration methodDeclaration2 = classDeclaration.AddMethod("AddRange");
				ParameterDeclaration p2 = methodDeclaration2.Signature.Parameters.Add(classDeclaration, name, nonNull: true);
				ForEachStatement forEachStatement = Stm.ForEach(Type, "item", Expr.Arg(p2), enumeratorDisposable: false);
				forEachStatement.Body.Add(Expr.This.Prop("List").Method("Add").Invoke(forEachStatement.Local));
				methodDeclaration2.Body.Add(forEachStatement);
			}
			if (Contains)
			{
				MethodDeclaration methodDeclaration3 = classDeclaration.AddMethod("Contains");
				methodDeclaration3.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
				ParameterDeclaration parameterDeclaration2 = methodDeclaration3.Signature.Parameters.Add(Type, name, nonNull: true);
				methodDeclaration3.Body.Return(Expr.This.Prop("List").Method("Contains").Invoke(parameterDeclaration2));
			}
			if (Remove)
			{
				MethodDeclaration methodDeclaration4 = classDeclaration.AddMethod("Remove");
				ParameterDeclaration parameterDeclaration3 = methodDeclaration4.Signature.Parameters.Add(Type, name, nonNull: true);
				methodDeclaration4.Doc.Summary.AddText("Removes the first occurrence of a specific ParameterDeclaration from this ParameterDeclarationCollection.");
				methodDeclaration4.Body.Add(Expr.This.Prop("List").Method("Remove").Invoke(parameterDeclaration3));
			}
			if (Insert)
			{
				MethodDeclaration methodDeclaration5 = classDeclaration.AddMethod("Insert");
				ParameterDeclaration parameterDeclaration4 = methodDeclaration5.Signature.Parameters.Add(typeof(int), "index", nonNull: true);
				ParameterDeclaration parameterDeclaration5 = methodDeclaration5.Signature.Parameters.Add(Type, name, nonNull: true);
				methodDeclaration5.Body.Add(Expr.This.Prop("List").Method("Insert").Invoke(parameterDeclaration4, parameterDeclaration5));
			}
			if (IndexOf)
			{
				MethodDeclaration methodDeclaration6 = classDeclaration.AddMethod("IndexOf");
				ParameterDeclaration parameterDeclaration6 = methodDeclaration6.Signature.Parameters.Add(Type, name, nonNull: true);
				methodDeclaration6.Signature.ReturnType = new TypeTypeDeclaration(typeof(int));
				methodDeclaration6.Body.Return(Expr.This.Prop("List").Method("IndexOf").Invoke(parameterDeclaration6));
			}
			if (Enumerator)
			{
				ClassDeclaration classDeclaration2 = classDeclaration.AddClass("Enumerator");
				FieldDeclaration fieldDeclaration = classDeclaration2.AddField(typeof(IEnumerator), "wrapped");
				classDeclaration2.Interfaces.Add(typeof(IEnumerator));
				ConstructorDeclaration constructorDeclaration = classDeclaration2.AddConstructor();
				ParameterDeclaration p3 = constructorDeclaration.Signature.Parameters.Add(classDeclaration, "collection", nonNull: true);
				constructorDeclaration.Body.Add(Stm.Assign(Expr.This.Field(fieldDeclaration), Expr.Arg(p3).Cast(typeof(CollectionBase)).Method("GetEnumerator")
					.Invoke()));
				PropertyDeclaration propertyDeclaration = classDeclaration2.AddProperty(Type, "Current");
				propertyDeclaration.Get.Return(Expr.This.Field(fieldDeclaration).Prop("Current").Cast(Type));
				PropertyDeclaration propertyDeclaration2 = classDeclaration2.AddProperty(typeof(object), "Current");
				propertyDeclaration2.Get.Return(Expr.This.Prop(propertyDeclaration));
				propertyDeclaration2.PrivateImplementationType = fieldDeclaration.Type;
				MethodDeclaration methodDeclaration7 = classDeclaration2.AddMethod("Reset");
				methodDeclaration7.ImplementationTypes.Add(fieldDeclaration.Type);
				methodDeclaration7.Body.Add(Expr.This.Field(fieldDeclaration).Method("Reset").Invoke());
				MethodDeclaration methodDeclaration8 = classDeclaration2.AddMethod("MoveNext");
				methodDeclaration8.ImplementationTypes.Add(fieldDeclaration.Type);
				methodDeclaration8.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
				methodDeclaration8.Body.Return(Expr.This.Field(fieldDeclaration).Method("MoveNext").Invoke());
				MethodDeclaration methodDeclaration9 = classDeclaration.AddMethod("GetEnumerator");
				methodDeclaration9.Attributes |= MemberAttributes.New;
				methodDeclaration9.ImplementationTypes.Add(new TypeTypeDeclaration(typeof(IEnumerable)));
				methodDeclaration9.Signature.ReturnType = classDeclaration2;
				methodDeclaration9.Body.Return(Expr.New(classDeclaration2, Expr.This));
			}
			return classDeclaration;
		}

		public override void Generate()
		{
			Prepare();
			AddClass(base.NamespaceDeclaration);
			Compile();
		}
	}
}
*/
