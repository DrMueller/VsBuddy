using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants
{
    public interface ITestSetupWriter
    {
        string WriteSetup(ClassInformation classInfo);
    }
}