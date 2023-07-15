using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services
{
    public interface ICsProjectFilesLocator
    {
        IReadOnlyCollection<string> GetAllCsProjFiles(string sourceFilePath);
    }
}
