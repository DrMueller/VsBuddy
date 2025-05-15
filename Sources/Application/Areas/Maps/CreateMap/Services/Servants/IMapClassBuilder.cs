using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;

namespace VsBuddy.Areas.Maps.CreateMap.Services.Servants
{
    public interface IMapClassBuilder
    {
        MethodDeclarationSyntax CreateMapMethod(ClassInformation classInfo, string mapClassName);

        SyntaxList<MemberDeclarationSyntax> CreateProperties(ClassInformation classInfo);
    }
}
