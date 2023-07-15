using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFrameworks.Models.Frameworks
{
    public class Nunit : ITestFramework
    {
        public TestAttribute ClassAttribute { get; } = TestAttribute.CreateFrom("Test");
        public TestClassSetup ClassSetup { get; } = new TestClassSetup(TestClassSetupType.Method, TestAttribute.CreateFrom("SetUp"));
        public string FrameworkName { get; } = "NUnit";
        public TestAttribute TestMethodAttribute { get; } = TestAttribute.CreateFrom("Test");
        public UsingEntry UsingEntry { get; } = UsingEntry.CreateFrom("NUnit.Framework");
    }
}