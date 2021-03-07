/*
using Refly.CodeDom;
using System;
using System.Collections;
using System.ComponentModel;

namespace Refly.Templates
{
	public class DictionaryTemplate : Template
	{
		[Category("Data")]
		public string Key = null;

		[Category("Data")]
		public string Value = null;

		[Category("Data")]
		public bool ItemGet = true;

		[Category("Data")]
		public bool ItemSet = true;

		[Category("Data")]
		public bool Add = true;

		[Category("Data")]
		public bool Contains = true;

		[Category("Data")]
		public bool Remove = true;

		protected ITypeDeclaration KeyType
		{
			get
			{
				if (Key == null)
				{
					throw new InvalidOperationException("Key type is undifned");
				}
				return new StringTypeDeclaration(Key);
			}
		}

		protected ITypeDeclaration ValueType
		{
			get
			{
				if (Value == null)
				{
					throw new InvalidOperationException("Value type is undifned");
				}
				return new StringTypeDeclaration(Value);
			}
		}

		protected string DictionaryName => string.Format(base.NameFormat, KeyType.Name, ValueType.Name);

		public DictionaryTemplate()
			: base("Strongly Typed Dictionary", "{0}{1}Dictionary")
		{
		}

		public ClassDeclaration AddClass(NamespaceDeclaration ns)
		{
			ClassDeclaration classDeclaration = ns.AddClass(DictionaryName);
			classDeclaration.Parent = new TypeTypeDeclaration(typeof(DictionaryBase));
			classDeclaration.AddConstructor();
			if (ItemGet || ItemSet)
			{
				IndexerDeclaration indexerDeclaration = classDeclaration.AddIndexer(ValueType);
				ParameterDeclaration p = indexerDeclaration.Signature.Parameters.Add(KeyType, "key", nonNull: false);
				if (ItemGet)
				{
					indexerDeclaration.Get.Return(Expr.This.Prop("Dictionary").Item(Expr.Arg(p)).Cast(ValueType));
				}
				if (ItemSet)
				{
					indexerDeclaration.Set.Add(Stm.Assign(Expr.This.Prop("Dictionary").Item(Expr.Arg(p)), Expr.Value));
				}
			}
			if (Add)
			{
				MethodDeclaration methodDeclaration = classDeclaration.AddMethod("Add");
				ParameterDeclaration parameterDeclaration = methodDeclaration.Signature.Parameters.Add(KeyType, "key", nonNull: true);
				ParameterDeclaration parameterDeclaration2 = methodDeclaration.Signature.Parameters.Add(ValueType, "value", nonNull: true);
				methodDeclaration.Body.Add(Expr.This.Prop("Dictionary").Method("Add").Invoke(parameterDeclaration, parameterDeclaration2));
			}
			if (Contains)
			{
				MethodDeclaration methodDeclaration2 = classDeclaration.AddMethod("Contains");
				methodDeclaration2.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
				ParameterDeclaration parameterDeclaration3 = methodDeclaration2.Signature.Parameters.Add(KeyType, "key", nonNull: true);
				methodDeclaration2.Body.Return(Expr.This.Prop("Dictionary").Method("Contains").Invoke(parameterDeclaration3));
			}
			if (Remove)
			{
				MethodDeclaration methodDeclaration3 = classDeclaration.AddMethod("Remove");
				ParameterDeclaration parameterDeclaration4 = methodDeclaration3.Signature.Parameters.Add(KeyType, "key", nonNull: true);
				methodDeclaration3.Body.Add(Expr.This.Prop("Dictionary").Method("Remove").Invoke(parameterDeclaration4));
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
