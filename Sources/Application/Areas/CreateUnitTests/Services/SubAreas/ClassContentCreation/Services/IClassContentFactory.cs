using VsBuddy.Areas.CreateUnitTests.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.CreateUnitTests.Services.SubAreas.ClassContentCreation.Services
{
    public interface IClassContentFactory
    {
        string CreateContent(
            ClassInformation classInfo,
            CsProj classCsProj,
            CsProj unitTestCsProj);
    }
}