﻿using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.CreateUnitTests.SubAreas.ClassContentCreation.Services
{
    public interface IClassContentFactory
    {
        string CreateContent(
            ClassInformation classInfo,
            CsProj classCsProj,
            CsProj unitTestCsProj);
    }
}