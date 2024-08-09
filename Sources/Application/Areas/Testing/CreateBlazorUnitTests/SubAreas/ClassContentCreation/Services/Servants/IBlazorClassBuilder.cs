using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;

namespace VsBuddy.Areas.Testing.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services.Servants
{
    public interface IBlazorClassBuilder
    {
        IBlazorClassBuilder AppendConstructor();
        IBlazorClassBuilder AppendExamplaryMethod();

        IBlazorClassBuilder AppendFields();

        ClassDeclarationSyntax Build();

        IBlazorClassBuilder Initialize(ClassInformation classInfo);
    }
}