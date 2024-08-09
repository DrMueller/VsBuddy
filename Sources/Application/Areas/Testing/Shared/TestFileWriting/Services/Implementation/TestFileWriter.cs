using System.IO.Abstractions;
using VsBuddy.Infrastructure.FilreWriting.Services;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Areas.Testing.Shared.TestFileWriting.Services.Implementation
{
    public class TestFileWriter : ITestFileWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileWriter _fileWriter;

        public TestFileWriter(
            IFileSystem fileSystem,
            IFileWriter fileWriter)
        {
            _fileSystem = fileSystem;
            _fileWriter = fileWriter;
        }

        public void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            CsProj unitTestsCsProj)
        {
            var rootNamespace = unitTestsCsProj.AssemblyName.Replace("UnitTests", string.Empty);

            var relativePath = classInfo
                .NamespaceDecl
                .Replace(rootNamespace, string.Empty)
                .Replace(".Implementation", string.Empty);

            var path = relativePath.Replace(".", "\\");
            var fullpath = _fileSystem.Path.Combine(unitTestsCsProj.AssemblyPath, path);

            var fullName = _fileSystem.Path.Combine(fullpath, $"{classInfo.ClassName}UnitTests.cs");

            _fileWriter.WriteFile(fullName, fileContent);
        }
    }
}