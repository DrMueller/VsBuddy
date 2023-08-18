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

        public bool CheckVerifyFilePath(string filePath)
        {
            if (_fileSystem.File.Exists(filePath))
            {
                _messageService.ShowMessage($"File '{filePath}' already existing.", MessageType.Warning);

                return true;
            }

            var dir = _fileSystem.Path.GetDirectoryName(filePath);

            if (!_fileSystem.Directory.Exists(dir))
            {
                _fileSystem.Directory.CreateDirectory(dir);
            }

            return false;
        }

        public void WriteFile(string filePath, string content)
        {
            if (CheckVerifyFilePath(filePath))
            {
                return;
            }

            if (_fileSystem.File.Exists(filePath))
            {
                _messageService.ShowMessage($"File '{filePath}' already existing.", MessageType.Warning);
                return;
            }

            _fileSystem.File.WriteAllText(filePath, content);
            _messageService.ShowMessage($"File '{filePath}' created.", MessageType.Info);
        }
    }
}