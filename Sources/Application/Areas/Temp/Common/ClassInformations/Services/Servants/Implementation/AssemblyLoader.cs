using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VsBuddy.Areas.Temp.Common.ClassInformations.Services.Servants.Implementation
{
    public class AssemblyLoader : IAssemblyLoader
    {
        public Assembly Load(string classFilePath)
        {
            var code = File.ReadAllText(classFilePath);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new MetadataReference[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var compilation = CSharpCompilation.Create(
                "AssemblyName",
                new[] { syntaxTree },
                references,
                options);

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    throw new Exception("Error, see consoel log");
                }

                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());

                return assembly;
            }
        }
    }
}