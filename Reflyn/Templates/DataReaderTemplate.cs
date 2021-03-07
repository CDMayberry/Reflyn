/*
using Refly.CodeDom;
using Refly.CodeDom.Statements;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace Refly.Templates
{
	public class DataReaderTemplate : Template
	{
		[Category("Data")]
		public bool Enumerator = true;

		[Category("Data")]
		public DataTemplate Data = new DataTemplate();

		public string DataReaderName => string.Format(base.NameFormat, Data.DataName);

		public DataReaderTemplate()
			: base("Strongly Typed Data Reader", "{0}DataReader")
		{
		}

		public override void Generate()
		{
			Data.NamespaceDeclaration = base.NamespaceDeclaration;
			Data.Generate();
			base.NamespaceDeclaration.Imports.Add("System.Data");
			ClassDeclaration classDeclaration = base.NamespaceDeclaration.AddClass(DataReaderName);
			classDeclaration.Interfaces.Add(typeof(IDisposable));
			FieldDeclaration<ClassDeclaration> fieldDeclaration = classDeclaration.AddField(typeof(IDataReader), "dr");
			var fieldDeclaration2 = classDeclaration.AddField(Data.DataName, "data");
			fieldDeclaration2.InitExpression = Expr.New(fieldDeclaration2.Type);
			var propertyDeclaration = classDeclaration.AddProperty(fieldDeclaration2, hasGet: true, hasSet: false, checkNonNull: false);
			IDictionaryEnumerator enumerator = Data.Properties.GetEnumerator();
			while (enumerator.MoveNext())
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)dictionaryEntry.Key;
				var propertyDeclaration2 = (PropertyDeclaration<ClassDeclaration>)dictionaryEntry.Value;
				PropertyDeclaration<ClassDeclaration> propertyDeclaration3 = classDeclaration.AddProperty(propertyDeclaration2.Type, propertyDeclaration2.Name);
				propertyDeclaration3.Get.Return(Expr.This.Field(fieldDeclaration2).Prop(propertyDeclaration2));
			}
			ConstructorDeclaration constructorDeclaration = classDeclaration.AddConstructor();
			ParameterDeclaration parameterDeclaration = constructorDeclaration.Signature.Parameters.Add(fieldDeclaration.Type, "dr", nonNull: false);
			constructorDeclaration.Body.Add(Stm.ThrowIfNull(parameterDeclaration));
			constructorDeclaration.Body.Add(Stm.Assign(Expr.This.Field(fieldDeclaration), Expr.Arg(parameterDeclaration)));
			MethodDeclaration methodDeclaration = classDeclaration.AddMethod("Close");
			methodDeclaration.Body.Add(Stm.IfNull(Expr.This.Field(fieldDeclaration), Stm.Return()));
			methodDeclaration.Body.Add(Expr.This.Field(fieldDeclaration).Method("Close").Invoke());
			methodDeclaration.Body.AddAssign(Expr.This.Field(fieldDeclaration), Expr.Null);
			methodDeclaration.Body.AddAssign(Expr.This.Field(fieldDeclaration2), Expr.Null);
			MethodDeclaration methodDeclaration2 = classDeclaration.AddMethod("Read");
			methodDeclaration2.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
			ConditionStatement value = Stm.IfIdentity(Expr.This.Field(fieldDeclaration).Method("Read").Invoke(), Expr.False, Stm.ToStm(Expr.This.Method(methodDeclaration).Invoke()), Stm.Return(Expr.False));
			methodDeclaration2.Body.Add(value);
			IDictionaryEnumerator enumerator2 = Data.Properties.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				DictionaryEntry dictionaryEntry3 = (DictionaryEntry)enumerator2.Current;
				DictionaryEntry dictionaryEntry4 = (DictionaryEntry)dictionaryEntry3.Key;
				PropertyDeclaration prop = (PropertyDeclaration)dictionaryEntry3.Value;
				methodDeclaration2.Body.AddAssign(Expr.This.Field(fieldDeclaration2).Prop(prop), Expr.This.Field(fieldDeclaration).Item(Expr.Prim(dictionaryEntry4.Key.ToString())).Cast(dictionaryEntry4.Value.ToString()));
			}
			methodDeclaration2.Body.Return(Expr.True);
			MethodDeclaration methodDeclaration3 = classDeclaration.AddMethod("Dispose");
			methodDeclaration3.ImplementationTypes.Add(typeof(IDisposable));
			methodDeclaration3.Body.Add(Expr.This.Method(methodDeclaration).Invoke());
			if (Enumerator)
			{
				AddEnumerator(classDeclaration, fieldDeclaration2, methodDeclaration);
			}
		}

		private void AddEnumerator(ClassDeclaration c, FieldDeclaration data, MethodDeclaration close)
		{
			c.Interfaces.Add(typeof(IEnumerable));
			ClassDeclaration classDeclaration = c.AddClass("Enumerator");
			FieldDeclaration fieldDeclaration = classDeclaration.AddField(c, "wrapped");
			ITypeDeclaration typeDeclaration = new TypeTypeDeclaration(typeof(IEnumerator));
			ITypeDeclaration value = new TypeTypeDeclaration(typeof(IDisposable));
			classDeclaration.Interfaces.Add(typeDeclaration);
			classDeclaration.Interfaces.Add(value);
			ConstructorDeclaration constructorDeclaration = classDeclaration.AddConstructor();
			ParameterDeclaration p = constructorDeclaration.Signature.Parameters.Add(c, "collection", nonNull: true);
			constructorDeclaration.Body.AddAssign(Expr.This.Field(fieldDeclaration), Expr.Arg(p));
			PropertyDeclaration propertyDeclaration = classDeclaration.AddProperty(data.Type, "Current");
			propertyDeclaration.Get.Return(Expr.This.Field(fieldDeclaration).Prop("Data"));
			PropertyDeclaration propertyDeclaration2 = classDeclaration.AddProperty(typeof(object), "Current");
			propertyDeclaration2.Get.Return(Expr.This.Prop(propertyDeclaration));
			propertyDeclaration2.PrivateImplementationType = typeDeclaration;
			MethodDeclaration methodDeclaration = classDeclaration.AddMethod("Reset");
			methodDeclaration.ImplementationTypes.Add(fieldDeclaration.Type);
			methodDeclaration.Body.Add(Stm.Throw(typeof(InvalidOperationException), Expr.Prim("Not supported")));
			MethodDeclaration methodDeclaration2 = classDeclaration.AddMethod("MoveNext");
			methodDeclaration2.ImplementationTypes.Add(fieldDeclaration.Type);
			methodDeclaration2.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
			methodDeclaration2.Body.Return(Expr.This.Field(fieldDeclaration).Method("Read").Invoke());
			MethodDeclaration methodDeclaration3 = classDeclaration.AddMethod("Dispose");
			methodDeclaration3.ImplementationTypes.Add(value);
			methodDeclaration3.Body.Add(Expr.This.Field(fieldDeclaration).Method(close).Invoke());
			methodDeclaration3.Body.AddAssign(Expr.This.Field(fieldDeclaration), Expr.Null);
			MethodDeclaration methodDeclaration4 = c.AddMethod("GetEnumerator");
			methodDeclaration4.Signature.ReturnType = classDeclaration;
			methodDeclaration4.Body.Return(Expr.New(classDeclaration, Expr.This));
			MethodDeclaration methodDeclaration5 = c.AddMethod("GetEnumerator");
			methodDeclaration5.PrivateImplementationType = new TypeTypeDeclaration(typeof(IEnumerable));
			methodDeclaration5.Signature.ReturnType = new TypeTypeDeclaration(typeof(IEnumerator));
			methodDeclaration5.Body.Return(Expr.This.Method("GetEnumerator").Invoke());
		}
	}
}
*/
