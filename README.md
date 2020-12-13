# Reflyn
A code-gen wrapper library built around Roslyn, based on Peli de Halleux's Refly library.

## Origin
The Code Project article for Refly can be found [here](https://www.codeproject.com/Articles/6283/Refly-makes-the-CodeDom-er-life-easier).

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

## License
This library is under the MIT License.
