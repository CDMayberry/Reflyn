using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Mixins;
using Reflyn.Test.Dummy;
using Xunit;

namespace Reflyn.Test
{
    public interface SampleState
    {
        bool IsGrounded { get; set; }
        string TestStr { get; set; }
    }

    // Basically using this as a console to just test whatever I'm working on.
    public class DebugTest
    {
        // This is more of a Compiler and Run test, if it all builds and runs it passes.
        [Fact]
        public void Test1()
        {
            var state = typeof(SampleState);
            string name = state.Name.Substring(1);

            var demo = new NamespaceDeclaration(state.Namespace + ".Generated");
            demo.AddImport("System");
            demo.AddNamespace("System");
            ClassDeclaration stateClass = demo.AddClass(name);
            stateClass.AddField(typeof(int), "TestInt");
            stateClass.AddField(typeof(bool), "TestBool");
            //.SetParent<ControllableState>();
            stateClass
                .AddCustomAttribute<DummyCustomAttribute>();
            stateClass
                .AddCustomAttribute<DummyPositionalCustomAttribute>()
                .Arguments.AddPositional(Expr.TypeOf<AssignmentExpressionSyntax>());
            stateClass
                .AddCustomAttribute<DummyNamedCustomAttribute>()
                .Arguments.Add("Str", Expr.Prim("Test"));

            var scriptableRef = Stm.Var(name, "scriptable", Expr.Type(typeof(ClassDeclaration)).GenericMethod("CreateInstance", name).Invoke());
            /*Stm.Var(name, "scriptable",
                Expr.Type(typeof(ClassDeclaration)).GenericMethod("CreateInstance", name).Invoke());*/
            var converter = stateClass
                .AddMethod("From" + name)
                .ToStatic()
                .SetReturnType(name)
                .WithParameter(name+"_", "arg")
                .Add(scriptableRef);

            List <PropertyInfo> props = state.GetProperties().Where(IsAutoProperty).ToList();
            foreach (var prop in props)
            {
                stateClass
                    .AddField(prop.PropertyType, prop.Name)
                    .ToPublic();
                
                //.AddCustomAttribute<HideInInspectorAttribute>();
            }


            /*MethodDeclaration writeMethod = stateClass
                .AddMethod("Write")
                .ToOverride()
                .AddParameter<BaseEntity>("entity")
                .AddParameter<NetOutgoingMessage>("message")
                .AddParameter<bool>("isSpawning")
                .CallBaseWithParameters();

            var message = writeMethod.Signature["message"];

            foreach (var field in stateClass.Fields)
            {
                writeMethod.Add(message.Method("Write").Invoke(field));
            }

            var readMethod = stateClass
                .AddMethod("Read")
                .ToOverride()
                .AddParameter<BaseEntity>("entity")
                .AddParameter<NetIncomingMessage>("message")
                .AddParameter<bool>("isSpawning")
                .CallBaseWithParameters();*/
            var comp = demo.ToCompilationUnit();
            var str = comp.NormalizeWhitespace().ToFullString();
            Assert.True(true);
        }

        // Comparing to directly writing in Roslyn, courtesy of https://carlos.mendible.com/2017/03/02/create-a-class-with-net-core-and-roslyn/
        [Fact]
        public void Test2()
        {
            var demo = new NamespaceDeclaration("CodeGenerationSample");

            var clss = demo.AddClass("Order")
                .SetBaseType("BaseEntity<Order>")
                .AddInterface("IHaveIdentity");

            var canceledField = clss.AddField(typeof(bool), "canceled");
            var intProp = clss.AddAutoProperty(typeof(int), "Quantity");

            var setTrue = clss.AddMethod("MarkAsCanceled").ToPublic().AddAssign(canceledField, Expr.True);

            var str = demo.ToCompilationUnit().NormalizeWhitespace().ToFullString();
            Assert.True(true);
        }

        public static bool IsAutoProperty(PropertyInfo prop)
        {
            return prop != null && prop.CanRead && prop.CanWrite;
        }
    }
}
