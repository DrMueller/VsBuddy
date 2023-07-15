using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFileWriting.Services
{
    public interface ITestFileWriter
    {
        void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            string filePath);
    }
}