using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Services
{
    public interface IClassInformationFactory
    {
        ClassInformation Create(string filePath, CsProj project);
    }
}