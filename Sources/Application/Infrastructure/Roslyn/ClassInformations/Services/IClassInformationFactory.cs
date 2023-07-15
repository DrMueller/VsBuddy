using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Services
{
    public interface IClassInformationFactory
    {
        ClassInformation Create(string filePath);
    }
}