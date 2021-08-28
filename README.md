# EnumFastToStringDotNet

This is a Visual Studio C# source generator for automatically generating enum extension methods that implement a switch expression based ToString method. Why? The default enum ToString method implements a binary search which is great for large lists but becomes time and memory inefficient for a just a few items when compared to a switch expression. Binary search has an O(log n) while a switch expression in this case has a O(1).

You can thank [Nick Chapsas](https://github.com/Elfocrash) on YouTube for making me aware of this. Check out his awesome video here: [C#'s Enum performance trap your code is suffering from](https://www.youtube.com/watch?v=BoE5Y6Xkm6w)

## Benchmark
Benchmark done with [BenchmarkDotNet](https://benchmarkdotnet.org/). You can run it for yourself by opening the solution in Visual Studio and running the samples/Benchmark project.
```
|                         Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|----------:|
|                 NativeToString | 25.871 ns | 0.5217 ns | 0.4625 ns | 0.0057 |      24 B |
|        SwitchStatementToString |  1.265 ns | 0.0512 ns | 0.0479 ns |      - |         - |
|       SwitchExpressionToString |  1.399 ns | 0.0565 ns | 0.0529 ns |      - |         - |
| EnumFastToStringDotNetToString |  1.510 ns | 0.0224 ns | 0.0175 ns |      - |         - |
```

## Quick Start

1. Add source generator reference to your project
```
  <ItemGroup>
    <ProjectReference Include="path\to\EnumFastToStringDotNet.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
  </ItemGroup>
```
2. Add using statement
```
using EnumFastToStringGenerated;
```
3. Add [FastToString] attribute to your enum
```
[FastToString]
public enum HumanStates
{
    Idle,
    Working,
    Sleeping,
    Eating,
    Dead
}
```
4. Call FastToString method
```
Console.Out.WriteLine(HumanStates.Dead.FastToString());
```

5. The Automatically Generated Code
```
using System;
namespace EnumFastToStringGenerated
{
    public static class EnumExtensions
    {
        public static string FastToString(this Namespace.HumanStates states)
        {
            return states switch
            {
                Namespace.HumanStates.Idle => nameof(Namespace.HumanStates.Idle),
                Namespace.HumanStates.Working => nameof(Namespace.HumanStates.Working),
                Namespace.HumanStates.Sleeping => nameof(Namespace.HumanStates.Sleeping),
                Namespace.HumanStates.Eating => nameof(Namespace.HumanStates.Eating),
                Namespace.HumanStates.Dead => nameof(Namespace.HumanStates.Dead),
                _ => throw new ArgumentOutOfRangeException(nameof(states), states, null)
            };
        }
    }
}
```

## Custom Method Name

If you would like to use a different method name, you can do so with the attribute argument "MethodName".
```
[FastToString(MethodName = "CustomMethodName")]
public enum HumanStates
{
    Idle,
    Working,
    Sleeping,
    Eating,
    Dead
}
```
Then you can call the method like this:
```
Console.Out.WriteLine(HumanStates.Dead.CustomMethodName());
```

## Customizing Source Generator

You can customize the source generator by changing the const values in the EnumSwitchSourceGenerator.cs file.
```
//The namespace that the attribute and extension method classes are defined in
private const string NAMESPACE = "EnumFastToStringGenerated";

//The name of the attribute
private const string ATTRIBUTE_NAME = "FastToString";

//The name of the extension method
private const string EXTENSION_METHOD_NAME = "FastToString";
```
