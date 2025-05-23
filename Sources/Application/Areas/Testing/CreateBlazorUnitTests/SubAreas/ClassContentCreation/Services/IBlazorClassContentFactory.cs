﻿using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services
{
    public interface IBlazorClassContentFactory
    {
        string CreateContent(
            ClassInformation classInfo,
            CsProj classCsProj,
            CsProj unitTestCsProj);
    }
}