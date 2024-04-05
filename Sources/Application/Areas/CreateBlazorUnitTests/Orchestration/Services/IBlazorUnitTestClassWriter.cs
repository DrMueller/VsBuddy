namespace VsBuddy.Areas.CreateBlazorUnitTests.Orchestration.Services
{
    public interface IBlazorUnitTestClassWriter
    {
        void CreateTestClass(string fileToTestPath);
    }
}