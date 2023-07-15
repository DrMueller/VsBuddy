using System.Collections.Generic;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services
{
    public interface ICsProjectFilesLocator
    {
        IReadOnlyCollection<string> GetAllCsProjFiles(string sourceFilePath);
    }
}