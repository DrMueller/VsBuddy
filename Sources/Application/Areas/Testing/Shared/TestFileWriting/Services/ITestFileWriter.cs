using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.Shared.TestFileWriting.Services
{
    public interface ITestFileWriter
    {
        void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            CsProj unitTestsCsProj);
    }
}