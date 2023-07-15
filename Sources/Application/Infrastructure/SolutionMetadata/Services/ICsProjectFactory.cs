using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services
{
    public interface ICsProjectFactory
    {
        CsProj Create(string filePath);
    }
}