using Microsoft.VisualStudio.OLE.Interop;
using VsBuddy.Areas.Temp.ClassWriting.SubAreas.ClassContentCreation.Services;
using VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFileWriting.Services;
using VsBuddy.Areas.Temp.Common.ClassInformations.Services;

namespace VsBuddy.Areas.Temp.ClassWriting.Orchestration.Services.Implementation
{
    public class UnitTestClassWriter : IUnitTestClassWriter
    {
        private readonly IClassContentFactory _classContentFactory;
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly ITestFileWriter _testFileWriter;

        public UnitTestClassWriter(
            IClassInformationFactory classInfoFactory,
            IClassContentFactory classContentFactory,
            ITestFileWriter testFileWriter)
        {
            _classInfoFactory = classInfoFactory;
            _classContentFactory = classContentFactory;
            _testFileWriter = testFileWriter;
        }

        public void CreateTestClass(string fileToTestPath)
        {
            var classInfo = _classInfoFactory.Create(fileToTestPath);
            var classContent = _classContentFactory.CreateContent(classInfo);
            _testFileWriter.WriteToTestLocation(classInfo, classContent, fileToTestPath);
        }
    }
}