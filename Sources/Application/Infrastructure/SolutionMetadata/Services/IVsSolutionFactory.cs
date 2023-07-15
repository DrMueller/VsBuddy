using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services
{
    public interface IVsSolutionFactory
    {
        VsSolution Create(string sourceFilePath);
    }
}