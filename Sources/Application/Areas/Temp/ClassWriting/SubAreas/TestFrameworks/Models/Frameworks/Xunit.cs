using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFrameworks.Models.Frameworks
{
    public class Xunit : ITestFramework
    {
        public TestAttribute ClassAttribute { get; } = TestAttribute.CreateNone();
        public TestClassSetup ClassSetup { get; } = new TestClassSetup(TestClassSetupType.Constructor, TestAttribute.CreateNone());
        public string FrameworkName { get; } = "XUnit";
        public TestAttribute TestMethodAttribute { get; } = TestAttribute.CreateFrom("Fact");
        public UsingEntry UsingEntry { get; } = UsingEntry.CreateFrom("Xunit");
    }
}