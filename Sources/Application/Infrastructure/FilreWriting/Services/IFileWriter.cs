namespace VsBuddy.Infrastructure.FilreWriting.Services
{
    public interface IFileWriter
    {
        bool CheckVerifyFilePath(string filePath);
        void WriteFile(string filePath, string content);
    }
}