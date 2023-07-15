using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.CreateUnitTests.ClassInformations.Models;

namespace VsBuddy.Areas.CreateUnitTests.Services.SubAreas.ClassContentCreation.Services.Servants
{
    public interface IClassBuilder
    {
        IClassBuilder AppendExamplaryMethod();

        IClassBuilder AppendFields();

        IClassBuilder AppendSetupMethod();

        ClassDeclarationSyntax Build();

        IClassBuilder Initialize(ClassInformation classInfo);
    }
}