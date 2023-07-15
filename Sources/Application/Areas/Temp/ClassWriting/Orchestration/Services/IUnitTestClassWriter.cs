using Microsoft.VisualStudio.OLE.Interop;

namespace VsBuddy.Areas.Temp.ClassWriting.Orchestration.Services
{
    public interface IUnitTestClassWriter
    {
        void CreateTestClass(string fileToTestPath);
    }
}