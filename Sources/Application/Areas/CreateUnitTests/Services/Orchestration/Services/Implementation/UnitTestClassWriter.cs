using VsBuddy.Areas.CreateUnitTests.ClassInformations.Services;
using VsBuddy.Areas.CreateUnitTests.Services.SubAreas.ClassContentCreation.Services;
using VsBuddy.Areas.CreateUnitTests.Services.SubAreas.TestFileWriting.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services.Implementation;

namespace VsBuddy.Areas.CreateUnitTests.Services.Orchestration.Services.Implementation
{
    public class UnitTestClassWriter : IUnitTestClassWriter
    {
        private readonly IClassContentFactory _classContentFactory;
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly ITestFileWriter _testFileWriter;
        private readonly IVsSolutionFactory _vsSolutionFactory;

        public UnitTestClassWriter(
            IClassInformationFactory classInfoFactory,
            IClassContentFactory classContentFactory,
            ITestFileWriter testFileWriter,
            IVsSolutionFactory vsSolutionFactory)
        {
            _classInfoFactory = classInfoFactory;
            _classContentFactory = classContentFactory;
            _testFileWriter = testFileWriter;
            _vsSolutionFactory = vsSolutionFactory;
        }

        public void CreateTestClass(string fileToTestPath)
        {
            var vsSolution = _vsSolutionFactory.Create(fileToTestPath);
            var classInfo = _classInfoFactory.Create(fileToTestPath);
            var unitTestsCsProj = vsSolution.SearchbyNamespace(classInfo.NamespaceDecl, "UnitTests");

            var classCsProj = vsSolution.SearchbyNamespace(classInfo.NamespaceDecl);
            
            var classContent = _classContentFactory.CreateContent(classInfo, classCsProj, unitTestsCsProj);
            _testFileWriter.WriteToTestLocation(classInfo, classContent, fileToTestPath, unitTestsCsProj);
        }
    }
}