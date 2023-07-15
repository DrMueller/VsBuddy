using System.IO.Abstractions;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Infrastructure.FilreWriting.Services.Implementation
{
    public class FileWriter : IFileWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMessageService _messageService;

        public FileWriter(
            IMessageService messageService,
            IFileSystem fileSystem)
        {
            _messageService = messageService;
            _fileSystem = fileSystem;
        }

        public void WriteFile(string filePath, string content)
        {
            if (CheckVerifyFilePath(filePath))
            {
                return;
            }

            _fileSystem.File.WriteAllText(filePath, content);

            _messageService.ShowMessage($"File '{filePath}' created.", MessageType.Info);
        }

        public bool CheckVerifyFilePath(string filePath)
        {
            if (_fileSystem.File.Exists(filePath))
            {
                _messageService.ShowMessage($"File '{filePath}' already existing.", MessageType.Warning);

                return true;
            }

            if (!_fileSystem.Directory.Exists(filePath))
            {
                _fileSystem.Directory.CreateDirectory(filePath);
            }

            return false;

        }
    }
}