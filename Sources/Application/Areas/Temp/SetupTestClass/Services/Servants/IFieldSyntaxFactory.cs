using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants
{
    public interface IFieldSyntaxFactory
    {
        IReadOnlyCollection<MemberDeclarationSyntax> CreateFields(ClassInformation classInfo);
    }
}