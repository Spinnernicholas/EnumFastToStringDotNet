# EnumFastToStringDotNet

This is a Visual Studio C# source generator for automatically generating enum extension methods that implement a switch expression based ToString method. Why? The default enum ToString method implements a binary search which is great for large lists but becomes time and memory inefficient for a just a few items when compared to a switch expression. Binary search has an O(log n) while a switch expression in this case has a O(1).

You can thank Nick Shapsas on YouTube for making me aware of this. Check out his awesome video here: [C#'s Enum performance trap your code is suffering from](https://www.youtube.com/watch?v=BoE5Y6Xkm6w)

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

##Customizing Source Generator

You can customize the source generator by changing the const values in the EnumSwitchSourceGenerator.cs file.
```
//The namespace that the attribute and extension method classes are defined in
private const string NAMESPACE = "EnumFastToStringGenerated";

//The name of the attribute
private const string ATTRIBUTE_NAME = "FastToString";

//The name of the extension method
private const string EXTENTION_METHOD_NAME = "FastToString";
```
