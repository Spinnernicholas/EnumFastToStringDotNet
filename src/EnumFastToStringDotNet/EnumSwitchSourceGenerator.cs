using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumSwitchSourceGenerator
{
    [Generator]
    public class EnumSwitchSourceGenerator : ISourceGenerator
    {
        private const string NAMESPACE = "EnumFastToStringGenerated";
        private const string ATTRIBUTE_NAME = "FastToString";
        private const string EXTENTION_METHOD_NAME = "FastToString";

        public void Execute(GeneratorExecutionContext context)
        {
            AddAttributeSource(context);
            ProcessAttributes(context);
        }

        public void ProcessAttributes(GeneratorExecutionContext context)
        {
            List<EnumDeclarationSyntax> enums = new List<EnumDeclarationSyntax>();

            foreach (SyntaxTree syntaxTree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                enums.AddRange(syntaxTree.GetRoot().DescendantNodesAndSelf()
                     .OfType<EnumDeclarationSyntax>()
                     .Where(x => semanticModel.GetDeclaredSymbol(x).GetAttributes()
                        .Any(x => x.AttributeClass.Name == ATTRIBUTE_NAME)));
            }

            StringBuilder sourceBuilder = new StringBuilder($@"using System;
namespace {NAMESPACE}
{{
    public static class EnumExtensions
    {{");
            foreach (EnumDeclarationSyntax e in enums)
            {
                var semanticModel = context.Compilation.GetSemanticModel(e.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(e);
                var symbolName = $"{symbol.ContainingNamespace}.{symbol.Name}";

                sourceBuilder.Append($@"
        public static string {EXTENTION_METHOD_NAME}(this {symbolName} states)
        {{
            return states switch
            {{
");
                foreach (string member in e.Members.Select(x => x.ToString()))
                    sourceBuilder.AppendLine($@"                {symbolName}.{member} => nameof({symbolName}.{member}),");
                sourceBuilder.Append(@"                _ => throw new ArgumentOutOfRangeException(nameof(states), states, null)
            };
        }");
            }
        sourceBuilder.Append(@"
    }
}
");
            context.AddSource("FastToStringGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private void AddAttributeSource(GeneratorExecutionContext context)
        {
            context.AddSource($"{ATTRIBUTE_NAME}Attribute", SourceText.From($@"using System;
namespace {NAMESPACE}
{{
    [AttributeUsage(AttributeTargets.Enum)]
    public class {ATTRIBUTE_NAME}Attribute : Attribute{{}}
}}
", Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
