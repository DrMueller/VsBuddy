using System.IO.Abstractions;
using System.Resources;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.CreateResx.SubAreas.ResxWriting.Services.Implementation
{
    public class ResxWriter : IResxWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMessageService _messageService;

        public ResxWriter(
            IMessageService messageService,
            IFileSystem fileSystem)
        {
            _messageService = messageService;
            _fileSystem = fileSystem;
        }

        public void WriteEmptyResx(string targetFilePath)
        {
            var path = _fileSystem.Path.GetDirectoryName(targetFilePath);

            if (!_fileSystem.Directory.Exists(path))
            {
                _fileSystem.Directory.CreateDirectory(path);
            }

            var fn = _fileSystem.Path.GetFileName(targetFilePath);

            if (fn.StartsWith("_"))
            {
                var newFn = fn.Substring(1);
                targetFilePath = _fileSystem.Path.Combine(_fileSystem.Path.GetDirectoryName(targetFilePath), newFn);
            }

            using (var resourceWriter = new ResXResourceWriter(targetFilePath))
            {
                resourceWriter.Generate();
            }

            _messageService.ShowMessage($"File '{targetFilePath}' created.", MessageType.Info);
        }
    }
}