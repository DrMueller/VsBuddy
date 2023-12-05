using System;
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

        private static bool ContainsSlnFile(DirectoryInfo dir)
        {
            return dir.GetFiles().Any(f => f.Name.EndsWith(".sln", StringComparison.OrdinalIgnoreCase));
        }

        private static DirectoryInfo GetSourcesDirectory(string sourceFilePath)
        {
            var currentDir = new DirectoryInfo(sourceFilePath).Parent;

            while (!IsSourcesFolder(currentDir) && !ContainsSlnFile(currentDir))
            {
                currentDir = currentDir.Parent;
            }

            return currentDir;
        }

        private static bool IsSourcesFolder(DirectoryInfo dir)
        {
            return dir.Name.EndsWith("Sources");
        }
    }
}