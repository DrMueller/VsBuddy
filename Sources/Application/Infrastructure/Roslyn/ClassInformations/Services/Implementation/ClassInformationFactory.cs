using System.IO.Abstractions;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;
using VsBuddy.Infrastructure.Types.Maybes;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Services.Implementation
{
    public class ClassInformationFactory : IClassInformationFactory
    {
        private readonly IFileSystem _fileSystem;

        public ClassInformationFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ClassInformation Create(string filePath, CsProj project)
        {
            var fileContent = _fileSystem.File.ReadAllText(filePath);
            var tree = CSharpSyntaxTree.ParseText(fileContent);
            var root = tree.GetRoot();

            var classDeclaration = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault();

            var fullNamespace = root
                .DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>().FirstOrDefault()
                ?.Name
                .ToString();

            if (string.IsNullOrEmpty(fullNamespace))
            {
                var path = _fileSystem.Path.GetDirectoryName(filePath);
                var relativePath = path.Replace(project.AssemblyPath, string.Empty);
                var replaced = relativePath.Replace("\\", ".");

                var ns = $"{project.AssemblyName}{replaced}";

                fullNamespace = ns;
            }

            var className = classDeclaration?.Identifier.Text;

            if (string.IsNullOrEmpty(className))
            {
                className = _fileSystem.Path.GetFileNameWithoutExtension(filePath);
            }

            var ctorDeclarations = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
            var ctors = ctorDeclarations.Select(
                ctorDecl =>
                {
                    var ctorParams = ctorDecl.DescendantNodes()
                        .OfType<ParameterSyntax>()
                        .Select(
                            f =>
                            {
                                var typeName = f.Type?.GetText().ToString();
                                var parameterName = f.Identifier.Text;

                                return new Parameter(typeName, parameterName);
                            })
                        .ToList();

                    return new Constructor(ctorParams);
                }).ToList();

            var usingEntries = root
                .DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(f => UsingEntry.CreateFrom(f.Name.ToString()))
                .ToList();

            var ctor = MaybeFactory.CreateFromNullable<Constructor>(ctors.FirstOrDefault());
            var classInfo = new ClassInformation(className, fullNamespace, ctor, usingEntries);

            return classInfo;
        }
    }
}