# Reflyn
A code-gen library built around Roslyn, based on Jonathan de Halleux's Refly library. 

## Source
The Code Project article for the original Refly can be found [here](https://www.codeproject.com/Articles/6283/Refly-makes-the-CodeDom-er-life-easier).

### Why rebuild Refly?
I've been learning about video game networking architecture, in the context of Unity. There's a lot of custom serialization that needs to be done, and it has to be done at least per object if not per field on a object. I wanted to have a way to define classes and fields via a inspector window or in code, and have that generated into a working state object, similar to Photon Bolt.

I found the Refly library and rather liked the design, but it was built before generics and so neither had support for nor itself used generics, on top of many other modern C# features. Separately, I had seen a number of projects that used Roslyn to generate code for the projects. On inspection, it became very hard to follow what Roslyn was doing and how exactly it was generating the code. I decided to try to keep the design of Refly, but improve it by swapping CodeDom out for Roslyn.

### Differences in Refly vs Reflyn
 * Reflyn is targeted at C#, Refly could technically also target VB. Reflyn could perhaps still be target at VB, but a number of the shortcuts and keywords are stringly-typed to C#.
 * Moved to a fluent style to make chaining easier.
 * Generic interfaces as Mixins are used for declarations to try minimize the inheritance. There is a lot of overlap of keywords but that then have different rules based on if it's applied to a field, class, property getter/setter, etc. 
 * It now uses generics within itself, and also allows the creation and usage of generics in the generated code. Generic generation is still a bit clunky, but is workable from my tests with it.
 * The addition of implicit operators, IE FieldDeclaration to FieldReferenceExpression, to simplify 

### Roslyn vs Reflyn
Take the following code as an example:
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
Pretty simple right? Here's what it looks like if it were directly coded in Roslyn, courtesy of [Roslyn Quoter](https://roslynquoter.azurewebsites.net/):
```csharp
CompilationUnit()
.WithUsings(
    SingletonList<UsingDirectiveSyntax>(
        UsingDirective(
            IdentifierName("System"))))
.WithMembers(
    SingletonList<MemberDeclarationSyntax>(
        NamespaceDeclaration(
            QualifiedName(
                IdentifierName("Demo"),
                IdentifierName("Generated")))
        .WithMembers(
            SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration("DemoClass")
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithMembers(
                    List<MemberDeclarationSyntax>(
                        new MemberDeclarationSyntax[]{
                            FieldDeclaration(
                                VariableDeclaration(
                                    PredefinedType(
                                        Token(SyntaxKind.IntKeyword)))
                                .WithVariables(
                                    SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(
                                            Identifier("TestInt"))))),
                            FieldDeclaration(
                                VariableDeclaration(
                                    PredefinedType(
                                        Token(SyntaxKind.BoolKeyword)))
                                .WithVariables(
                                    SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(
                                            Identifier("TestBool")))))}))))))
.NormalizeWhitespace()
```
That's a lot of code! All for a namespace containing a simple class with two fields. It could be nested less by using variables but it'd still have to make a lot of calls for the different types, tokens, etc.

Now let's generate the same thing using Reflyn:
```csharp
var demo = new NamespaceDeclaration("Demo.Generated");
demo.AddImport("System");

ClassDeclaration demoClass = demo.AddClass("DemoClass");
demoClass.AddField(typeof(int), "TestInt");
demoClass.AddField(typeof(bool), "TestBool");

var comp = demo.ToCompilationUnit();
var str = comp.NormalizeWhitespace().ToFullString();
```
Far easier to read!

### Current State
I'm happy enough with the current state to use it in my other projects. For instance, MirrorState uses this to generate State and Controller behaviours. It's still lacking some features, and the API isn't consistent in a few places, but I'm happy with it as is.

## Tasks
- [ ] Reduce usage of methods that take a string directly as a type. Encourage using Type, TypeTypeDeclaration, or StringTypeDeclaration. 
- [ ] Improve consistency of 'With' vs 'Add' methods. Sometimes Add is used to indicate the fluent api will return the child declaration and With will return the parent declaration, but in other places this is reversed.
- [ ] Improve unit tests. I have minimal tests at the moment.
- [ ] The API for writing generics isn't ideal. It can be a bit confusing when the generic takes generics, how it generates generics under the hood is a little hacky, overall needs some improvement.

## License
This library is under the MIT License.
