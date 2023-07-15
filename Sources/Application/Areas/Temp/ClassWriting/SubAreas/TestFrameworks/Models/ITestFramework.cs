using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFrameworks.Models
{
    public interface ITestFramework
    {
        TestAttribute ClassAttribute { get; }
        TestClassSetup ClassSetup { get; }

        string FrameworkName { get; }
        TestAttribute TestMethodAttribute { get; }
        UsingEntry UsingEntry { get; }
    }
}