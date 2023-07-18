using System.IO.Abstractions;
using System.Resources;
using VsBuddy.Infrastructure.FilreWriting.Services;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.CreateResx.SubAreas.ResxWriting.Services.Implementation
{
    public class ResxWriter : IResxWriter
    {
        private readonly IFileSystem _fileSstem;
        private readonly IFileWriter _fileWriter;
        private readonly IMessageService _messageService;

        public ResxWriter(
            IMessageService messageService,
            IFileWriter fileWriter,
            IFileSystem fileSstem)
        {
            _messageService = messageService;
            _fileWriter = fileWriter;
            _fileSstem = fileSstem;
        }

        public void WriteEmptyResx(string targetFilePath)
        {
            var path = _fileSstem.Path.GetDirectoryName(targetFilePath);

            if (_fileWriter.CheckVerifyFilePath(path))
            {
                return;
            }

            var fn = _fileSstem.Path.GetFileName(targetFilePath);

            if (fn.StartsWith("_"))
            {
                var newFn = fn.Substring(1);
                targetFilePath = _fileSstem.Path.Combine(_fileSstem.Path.GetDirectoryName(targetFilePath), newFn);
            }

            using (var resourceWriter = new ResXResourceWriter(targetFilePath))
            {
                resourceWriter.Generate();
            }

            _messageService.ShowMessage($"File '{targetFilePath}' created.", MessageType.Info);
        }
    }
}