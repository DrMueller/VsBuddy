using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFileWriting.Services.Implementation
{
    public class TestFileWriter : ITestFileWriter
    {
        private readonly IFileSystem _fileSystem;

        public TestFileWriter(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void WriteToTestLocation(
            ClassInformation classInfo,
            string fileContent)
        {
            //var fileName = classInfo.ClassName + "UnitTests.cs";

            //var relativeNamespace = classInfo.NamespaceDecl.Replace(testConfig.ApplicationProjectBaseNamespace, string.Empty);

            //var namespaceParts = relativeNamespace.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //var namespaceQueue = new Queue<string>(namespaceParts);

            //var currentDirInfo = _fileSystem
            //    .DirectoryInfo
            //    .FromDirectoryName(testConfig.TestProjectBasePath);

            //AssurePathExists(currentDirInfo, namespaceQueue);

            //var pathParts = new List<string>
            //{
            //    testConfig.TestProjectBasePath
            //}.Concat(namespaceParts).ToArray();

            //var fullPath = _fileSystem.Path.Combine(pathParts);

            //var fullFileName = _fileSystem.Path.Combine(fullPath, fileName);
            //_fileSystem.File.WriteAllText(fullFileName, fileContent);
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