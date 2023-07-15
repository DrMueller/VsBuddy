using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.ClassWriting.SubAreas.ClassContentCreation.Services.Servants;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.ClassContentCreation.Services.Implementation
{
    public class ClassContentFactory : IClassContentFactory
    {
        private readonly IClassBuilder _classBuilder;

        public ClassContentFactory(IClassBuilder classBuilder)
        {
            _classBuilder = classBuilder;
        }

        public string CreateContent(ClassInformation classInfo)
        {
            var cls = _classBuilder.Initialize(classInfo)
                .AppendFields()
                .AppendSetupMethod()
                .AppendExamplaryMethod()
                .Build();

            var nameSpace = CreateNamespace(classInfo);
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
            classInfo.AppendUsing(UsingEntry.CreateFrom("XUnit"));
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

        private static NamespaceDeclarationSyntax CreateNamespace(ClassInformation classInfo)
        {
            var relativeNamespacePart = classInfo
                .NamespaceDecl;
            //    .Replace(testConfig.ApplicationProjectBaseNamespace, string.Empty);

            //var nameSpace = testConfig.TestProjectBaseNamespace + relativeNamespacePart;
            var ns = SyntaxFactory
                .NamespaceDeclaration(SyntaxFactory.ParseName(relativeNamespacePart))
                .NormalizeWhitespace();

            return ns;
        }
    }
}