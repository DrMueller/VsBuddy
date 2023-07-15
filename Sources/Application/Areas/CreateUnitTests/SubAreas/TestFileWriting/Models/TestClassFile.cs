namespace VsBuddy.Areas.CreateUnitTests.SubAreas.TestFileWriting.Models
{
    public class TestClassFile
    {
        public string Content { get; }
        public string OriginalFileNamespace { get; }

        public TestClassFile(string originalFileNamespace, string content)
        {
            OriginalFileNamespace = originalFileNamespace;
            Content = content;
        }
    }
}