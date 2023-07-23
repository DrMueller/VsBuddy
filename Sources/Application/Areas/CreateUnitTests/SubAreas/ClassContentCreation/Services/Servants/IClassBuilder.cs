using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;

namespace VsBuddy.Areas.CreateUnitTests.SubAreas.ClassContentCreation.Services.Servants
{
    public interface IClassBuilder
    {
        IClassBuilder AppendConstructor();
        IClassBuilder AppendExamplaryMethod();

        IClassBuilder AppendFields();

        ClassDeclarationSyntax Build();

        IClassBuilder Initialize(ClassInformation classInfo);
    }
}