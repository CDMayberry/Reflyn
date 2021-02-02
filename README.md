# Reflyn
A code-gen wrapper library built around Roslyn, based on Peli de Halleux's Refly library. 


## Source
The Code Project article for the original Refly can be found [here](https://www.codeproject.com/Articles/6283/Refly-makes-the-CodeDom-er-life-easier).

This code base existed before generics were implemented in C#, and in fact one of the usages of Refly was to create collections 

### Why rebuild Refly?
I've been learning video game networking architecture, specifically within Unity. There's a lot of custom serialization that needs to be done, and it has to be done at least per object if not per field on a object. I wanted to have a way to define classes and fields via a user window or code and have that generated into a working state object, similar to Photon Bolt.

I found the Refly library and rather liked the design of the wrapper, but it was built long ago and neither had support for nor itself used generics, on top of many other modern C# features. I decided to try to keep the design of the wrapper, but improve it by swapping in Roslyn as the code generator and adding generics.

### Differences in Refly vs Reflyn
 * Reflyn is targeted at C#, Refly could technically also target VB and other potentially code bases. Reflyn could perhaps still be target at VB, but a number of the shortcuts and keywords are stringly-typed to C#.
 * Tried to convert to a fluent style to make chaining easier,
 * Mixins are used for declarations to try minimize the inheritance of declarations. There is a lot of overlap of keywords but that then have different rules based on if it's applied to a field, class, property getter/setter, etc. 
 * It now uses generics within itself, and also allows the creation and usage of generics in the generated code. This part is still a bit clunky, but is workable from my tests with it.
 * The addition of implicit operators, IE FieldDeclaration to FieldReferenceExpression
 * 

## Example
```csharp
var demo = new NamespaceDeclaration("Demo.Generated");
demo.AddImport("System");

ClassDeclaration demoClass = demo.AddClass("DemoClass");
demoClass.AddField(typeof(int), "TestInt");
demoClass.AddField(typeof(bool), "TestBool");

var comp = demo.ToCompilationUnit();
var str = comp.NormalizeWhitespace().ToFullString();
```
produces
```csharp
using System;

namespace Demo.Generated
{
    public class DemoClass
    {
        int TestInt;
        bool TestBool;
    }
}
```

## Tasks
- [ ] Reduce usage of methods that take a string as a type. Force using Type, TypeTypeDeclaration, or StringTypeDeclaration. 
- [ ] Improve consistency of 'With' vs 'Add' methods. Sometimes Add is used to indicate the fluent api will return the class being added and With will return the current class being added to, but in other places this is reversed.

## License
This library is under the MIT License.
