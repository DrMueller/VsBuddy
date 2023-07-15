﻿using System.IO.Abstractions;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;
using VsBuddy.Areas.Temp.Common.ClassInformations.Services.Servants;

namespace VsBuddy.Areas.Temp.Common.ClassInformations.Services.Implementation
{
    public class ClassInformationFactory : IClassInformationFactory
    {
        private readonly IAssemblyLoader _assemblyLoader;
        private readonly IFileSystem _fileSystem;

        public ClassInformationFactory(
            IAssemblyLoader assemblyLoader,
            IFileSystem fileSystem)
        {
            _assemblyLoader = assemblyLoader;
            _fileSystem = fileSystem;
        }

        public ClassInformation Create(string filePath)
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
                .OfType<NamespaceDeclarationSyntax>().First()
                .Name
                .ToString();

            var className = classDeclaration?.Identifier.Text;
            var ctorDeclarations = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
            var ctors = ctorDeclarations.Select(
                ctorDecl =>
                {
                    var ctorParams = ctorDecl.DescendantNodes()
                        .OfType<ParameterSyntax>()
                        .Select(
                            f =>
                            {
                                var typeName = ((IdentifierNameSyntax)f.Type).Identifier.Text;
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

            var assembly = _assemblyLoader.Load(filePath);

            var classInfo = new ClassInformation(className, fullNamespace, ctors.First(), usingEntries, assembly);
            return classInfo;
        }
    }
}