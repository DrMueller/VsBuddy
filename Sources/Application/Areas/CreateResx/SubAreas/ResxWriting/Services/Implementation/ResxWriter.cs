using System.IO.Abstractions;
using System.Resources;
using VsBuddy.Areas.CreateResx.SubAreas.ResxWriting;
using VsBuddy.Infrastructure.FilreWriting.Services;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.CreateResx.SubAreas.ResxCreating.Services.Implementation
{
    public class ResxWriter : IResxWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileSystem _fileSstem;
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

            using (var resourceWriter = new ResXResourceWriter(targetFilePath))
            {
                resourceWriter.Generate();
            }

            _messageService.ShowMessage($"File '{targetFilePath}' created.", MessageType.Info);
        }
    }
}