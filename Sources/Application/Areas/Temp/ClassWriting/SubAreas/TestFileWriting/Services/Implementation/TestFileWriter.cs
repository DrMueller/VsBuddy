using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Services;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFileWriting.Services.Implementation
{
    public class TestFileWriter : ITestFileWriter
    {
        private readonly IVsSolutionFactory _solutionFactory;
        private readonly IFileSystem _fileSystem;
        private readonly IMessageService _messageService;

        public TestFileWriter(
            IVsSolutionFactory solutionFactory,
            IFileSystem fileSystem,
            IMessageService messageService)
        {
            _solutionFactory = solutionFactory;
            _fileSystem = fileSystem;
            _messageService = messageService;
        }

        public void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            string filePath)
        {
            var vsSolution = _solutionFactory.Create(filePath);

            var unitTestsCsProj = vsSolution.SearchUnitTestsByClassNamespace(classInfo.NamespaceDecl);

            var rootPath = unitTestsCsProj.AssemblyName.Replace("UnitTests", string.Empty);
            var relativePath = classInfo.NamespaceDecl.Replace(rootPath, string.Empty);
            var path = relativePath.Replace(".", "\\");
            var fullpath = _fileSystem.Path.Combine(unitTestsCsProj.AssemblyPath, path);

            var fullName = _fileSystem.Path.Combine(fullpath, $"{classInfo.ClassName}UnitTests.cs");

            if (_fileSystem.File.Exists(fullName))
            {
                _messageService.ShowMessage($"File {fullName} already existing.");

                return;
            }
            
            _fileSystem.File.WriteAllText(fullName, fileContent);

            _messageService.ShowMessage($"File {fullName} created.");
        }

        private static void AssurePathExists(IDirectoryInfo dirInfo, Queue<string> nameSpaceParts)
        {
            if (!nameSpaceParts.Any())
            {
                return;
            }

            var nextPart = nameSpaceParts.Dequeue();
            var subDirectories = dirInfo.GetDirectories();
            var nextDirectory = subDirectories.SingleOrDefault(f => f.Name == nextPart);

            if (nextDirectory == null)
            {
                nextDirectory = dirInfo.CreateSubdirectory(nextPart);
            }

            AssurePathExists(nextDirectory, nameSpaceParts);
        }
    }
}