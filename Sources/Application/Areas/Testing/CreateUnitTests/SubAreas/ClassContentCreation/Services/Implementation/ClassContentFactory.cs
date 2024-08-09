using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Testing.CreateUnitTests.SubAreas.ClassContentCreation.Services.Servants;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.CreateUnitTests.SubAreas.ClassContentCreation.Services.Implementation
{
    public class ClassContentFactory : IClassContentFactory
    {
        private readonly IClassBuilder _classBuilder;

        public ClassContentFactory(IClassBuilder classBuilder)
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
            syntaxFactory = AppendUsings(syntaxFactory, classInfo);
            syntaxFactory = syntaxFactory.AddMembers(nameSpace);

            var classContent = syntaxFactory
                .NormalizeWhitespace()
                .ToFullString();

            return classContent;
        }

        private static CompilationUnitSyntax AppendUsings(
            CompilationUnitSyntax syntaxFactory,
            ClassInformation classInfo)
        {
            classInfo.AppendUsing(UsingEntry.CreateFrom("Moq"));
            classInfo.AppendUsing(UsingEntry.CreateFrom("Xunit"));
            classInfo.AppendUsing(UsingEntry.CreateFrom(classInfo.NamespaceDecl));

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