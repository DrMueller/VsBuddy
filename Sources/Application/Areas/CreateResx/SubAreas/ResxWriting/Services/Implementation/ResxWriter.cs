using System.IO.Abstractions;
using System.Resources;
using VsBuddy.Infrastructure.FilreWriting.Services;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.CreateResx.SubAreas.ResxWriting.Services.Implementation
{
    public class ResxWriter : IResxWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileWriter _fileWriter;
        private readonly IMessageService _messageService;

        public ResxWriter(
            IMessageService messageService,
            IFileSystem fileSystem,
            IFileWriter fileWriter)
        {
            _messageService = messageService;
            _fileSystem = fileSystem;
            _fileWriter = fileWriter;
        }

        public void WriteEmptyResx(string targetFilePath)
        {
            var fn = _fileSystem.Path.GetFileName(targetFilePath);

            if (fn.StartsWith("_"))
            {
                var newFn = fn.Substring(1);
                targetFilePath = _fileSystem.Path.Combine(_fileSystem.Path.GetDirectoryName(targetFilePath), newFn);
            }

            if (_fileWriter.CheckVerifyFilePath(targetFilePath))
            {
                return;
            }

            using (var resourceWriter = new ResXResourceWriter(targetFilePath))
            {
                resourceWriter.Generate();
            }

            _messageService.ShowMessage($"File '{targetFilePath}' created.", MessageType.Info);
        }
    }
}