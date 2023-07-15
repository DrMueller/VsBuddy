using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using VsBuddy.Areas.CreateUnitTests.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Services;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.CreateUnitTests.Services.SubAreas.TestFileWriting.Services.Implementation
{
    public class TestFileWriter : ITestFileWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMessageService _messageService;

        public TestFileWriter(
            IFileSystem fileSystem,
            IMessageService messageService)
        {
            _fileSystem = fileSystem;
            _messageService = messageService;
        }

        public void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent,
            string filePath,
            CsProj unitTestsCsProj)
        {
  
            var rootPath = unitTestsCsProj.AssemblyName.Replace("UnitTests", string.Empty);
            var relativePath = classInfo.NamespaceDecl.Replace(rootPath, string.Empty);
            var path = relativePath.Replace(".", "\\");
            var fullpath = _fileSystem.Path.Combine(unitTestsCsProj.AssemblyPath, path);

            var fullName = _fileSystem.Path.Combine(fullpath, $"{classInfo.ClassName}UnitTests.cs");

            if (_fileSystem.File.Exists(fullName))
            {
                _messageService.ShowMessage($"File {fullName} already existing.", Infrastructure.VisualStudio.Messaging.Models.MessageType.Warning);

                return;
            }

            if (!_fileSystem.Directory.Exists(fullpath))
            {
                _fileSystem.Directory.CreateDirectory(fullpath);
            }


            _fileSystem.File.WriteAllText(fullName, fileContent);

            _messageService.ShowMessage($"File {fullName} created.", Infrastructure.VisualStudio.Messaging.Models.MessageType.Info);
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