using VsBuddy.Areas.CreateUnitTests.ClassInformations.Models;

namespace VsBuddy.Areas.CreateUnitTests.ClassInformations.Services
{
    public interface IClassInformationFactory
    {
        ClassInformation Create(string filePath);
    }
}