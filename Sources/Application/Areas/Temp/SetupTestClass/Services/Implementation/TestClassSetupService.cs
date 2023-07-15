using System.Windows.Forms;
using VsBuddy.Areas.Temp.Common.ClassInformations.Services;
using VsBuddy.Areas.Temp.SetupTestClass.Services.Servants;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Implementation
{
    public class TestClassSetupService : ITestClassSetupService
    {
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly ITestSetupWriter _testSetupWriter;

        public TestClassSetupService(
            IClassInformationFactory classInfoFactory,
            ITestSetupWriter testSetupWriter)
        {
            _classInfoFactory = classInfoFactory;
            _testSetupWriter = testSetupWriter;
        }

        public void SetupTestClass(string filePath)
        {
            var classInfo = _classInfoFactory.Create(filePath);
            var str = _testSetupWriter.WriteSetup(classInfo);
            Clipboard.SetText(str);
        }
    }
}