using VsBuddy.Areas.Testing.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services;
using VsBuddy.Areas.Testing.Shared.TestFileWriting.Services;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services;

namespace VsBuddy.Areas.Testing.CreateBlazorUnitTests.Orchestration.Services.Implementation
{
    public class BlazorUnitTestClassWriter : IBlazorUnitTestClassWriter
    {
        private readonly IBlazorClassContentFactory _classContentFactory;
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly ITestFileWriter _testFileWriter;
        private readonly IVsSolutionFactory _vsSolutionFactory;

        public BlazorUnitTestClassWriter(
            IClassInformationFactory classInfoFactory,
            IBlazorClassContentFactory classContentFactory,
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
            var classCsProj = vsSolution.SearchCsProjByPath(fileToTestPath);

            var classInfo = _classInfoFactory.Create(fileToTestPath, classCsProj);
            var unitTestsCsProj = vsSolution.SearchByNamespace(classInfo.NamespaceDecl, "UnitTests");

            var classContent = _classContentFactory.CreateContent(classInfo, classCsProj, unitTestsCsProj);
            _testFileWriter.WriteToTestLocation(classInfo, classContent, unitTestsCsProj);
        }
    }
}