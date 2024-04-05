using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;
using VsBuddy.Infrastructure.Types.Maybes;
using VsBuddy.Infrastructure.Types.Maybes.Implementation;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Services.Implementation
{
    public class ClassInformationFactory : IClassInformationFactory
    {
        private readonly IFileSystem _fileSystem;

        public ClassInformationFactory(
            IFileSystem fileSystem)
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

            var usingEntries = root
                .DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(f => UsingEntry.CreateFrom(f.Name.ToString()))
                .ToList();

            var ctor = CreateConstructor(root);
            var injections = CreateInjections(root);

            var classInfo = new ClassInformation(className, fullNamespace, ctor, usingEntries, injections);

            return classInfo;
        }

        private static Maybe<Constructor> CreateConstructor(SyntaxNode root)
        {
            var ctorDeclarations = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
            var ctors = ctorDeclarations.Select(
                ctorDecl =>
                {
                    var ctorParams = ctorDecl.DescendantNodes()
                        .OfType<ParameterSyntax>()
                        .Select(MapSyntax)
                        .ToList();

                    return new Constructor(ctorParams);
                }).ToList();

            var ctor = ctors.FirstOrDefault();

            if (ctor == null)
            {
                return TryCreatingFromPrimaryConstructor(root);
            }

            return ctor;
        }

        private static IReadOnlyCollection<Parameter> CreateInjections(SyntaxNode root)
        {
            var properties = root.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>();

            var injectProperties = properties.Where(f => f.AttributeLists.Any(g => g.Attributes.Any(h => h.Name.ToString() == "Inject"))).ToList();

            return injectProperties.Select(f => new Parameter(f.Type.GetText().ToString(), f.Identifier.Text))
                .ToList();
        }

        private static Parameter MapSyntax(ParameterSyntax syntax)
        {
            return new Parameter(syntax.Type?.GetText().ToString(), syntax.Identifier.Text);
        }

        private static Maybe<Constructor> TryCreatingFromPrimaryConstructor(SyntaxNode root)
        {
            var classDeclarations = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .ToList();

            // Seems like blazor files behave diferently, let's skip
            if (classDeclarations.Count > 1)
            {
                return None.Value;
            }

            var classDeclaration = classDeclarations.SingleOrDefault();

            var paramsListSyntax = classDeclaration?.ChildNodes()
                .OfType<ParameterListSyntax>()
                .SingleOrDefault();

            if (paramsListSyntax == null)
            {
                return None.Value;
            }

            var parms = paramsListSyntax
                .ChildNodes()
                .OfType<ParameterSyntax>()
                .Select(MapSyntax)
                .ToList();

            return new Constructor(parms);
        }
    }
}