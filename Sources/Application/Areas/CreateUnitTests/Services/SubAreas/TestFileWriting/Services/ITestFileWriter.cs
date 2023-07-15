using VsBuddy.Areas.CreateUnitTests.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.CreateUnitTests.Services.SubAreas.TestFileWriting.Services
{
    public interface ITestFileWriter
    {
        void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            string filePath,
            CsProj unitTestsCsProj);
    }
}