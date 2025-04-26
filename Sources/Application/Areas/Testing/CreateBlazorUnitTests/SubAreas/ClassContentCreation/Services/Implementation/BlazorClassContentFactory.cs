using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Testing.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services.Servants;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services.Implementation
{
    public class BlazorClassContentFactory : IBlazorClassContentFactory
    {
        private readonly IBlazorClassBuilder _classBuilder;

        public BlazorClassContentFactory(IBlazorClassBuilder classBuilder)
        {
            _classBuilder = classBuilder;
        }

        public string CreateContent(
            ClassInformation classInfo,
            CsProj classCsProj,
            CsProj unitTestCsProj)
        {
            var cls = _classBuilder.Initialize(classInfo)
                .AppendFields()
                .AppendConstructor()
                .AppendExamplaryMethod()
                .Build();

            var nameSpace = CreateNamespace(classInfo, classCsProj, unitTestCsProj);
            nameSpace = nameSpace.AddMembers(cls);

            var syntaxFactory = SyntaxFactory.CompilationUnit();
            syntaxFactory = AppendUsings(syntaxFactory, classInfo, unitTestCsProj);
            syntaxFactory = syntaxFactory.AddMembers(nameSpace);

            var classContent = syntaxFactory
                .NormalizeWhitespace()
                .ToFullString();

            return classContent;
        }

        private static CompilationUnitSyntax AppendUsings(
            CompilationUnitSyntax syntaxFactory,
            ClassInformation classInfo,
            CsProj unitTestProject)
        {
            classInfo.AppendUsing(UsingEntry.CreateFrom("Bunit"));
            classInfo.AppendUsing(UsingEntry.CreateFrom("Microsoft.Extensions.DependencyInjection"));
            classInfo.AppendUsing(UsingEntry.CreateFrom("Moq"));
            classInfo.AppendUsing(UsingEntry.CreateFrom("Xunit"));

            classInfo.AppendUsing(UsingEntry.CreateFrom(classInfo.NamespaceDecl));

            var jsRefExtension = $"{unitTestProject.AssemblyName}.TestingInfrastructure";

            classInfo.AppendUsing(UsingEntry.CreateFrom(jsRefExtension));

            foreach (var usingName in classInfo.SortedUsingEntries)
            {
                syntaxFactory = syntaxFactory
                    .AddUsings(
                        SyntaxFactory.UsingDirective(
                            SyntaxFactory.ParseName(usingName.Value)));
            }

            return syntaxFactory;
        }

        private static NamespaceDeclarationSyntax CreateNamespace(ClassInformation classInfo,
            CsProj classCsProj,
            CsProj unitTestsCsProj)
        {
            var relativeNamespacePart = classInfo
                .NamespaceDecl
                .Replace(classCsProj.AssemblyName, unitTestsCsProj.AssemblyName)
                .Replace(".Implementation", string.Empty);

            var ns = SyntaxFactory
                .NamespaceDeclaration(SyntaxFactory.ParseName(relativeNamespacePart))
                .NormalizeWhitespace();

            return ns;
        }
    }
}