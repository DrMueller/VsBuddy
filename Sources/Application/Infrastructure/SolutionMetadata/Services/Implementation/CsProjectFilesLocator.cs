using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services.Implementation
{
    public class CsProjectFilesLocator : ICsProjectFilesLocator
    {
        public IReadOnlyCollection<string> GetAllCsProjFiles(string sourceFilePath)
        {
            var sourcesDir = GetSourcesDirectory(sourceFilePath);
            return Directory
                .GetFiles(sourcesDir.FullName, "*.csproj", SearchOption.AllDirectories)
                .ToList();
        }

        private static DirectoryInfo GetSourcesDirectory(string sourceFilePath)
        {
            var currentDir = new DirectoryInfo(sourceFilePath);

            while (!currentDir.FullName.EndsWith("Sources"))
            {
                currentDir = currentDir.Parent;
            }

            return currentDir;
        }
    }
}