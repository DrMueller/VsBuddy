using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;

namespace VsBuddy.Areas.ClearCss.Services.Implementation
{
    public class CssClearer : ICssClearer
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMessageService _messageService;

        public CssClearer(
            IFileSystem fileSystem,
            IMessageService messageService)
        {
            _fileSystem = fileSystem;
            _messageService = messageService;
        }

        public void Clear(string solutionFilePath)
        {
            var solutionDir = _fileSystem.Path.GetDirectoryName(solutionFilePath);
            DeleteCssFiles(solutionDir);
            _messageService.ShowMessage("CSS cleared.", MessageType.Info);
        }

        private void DeleteCssFiles(string directoryPath)
        {
            Debug.WriteLine(directoryPath);
            var dirInfo = _fileSystem.DirectoryInfo.New(directoryPath);

            // Not cleaning external css
            if (dirInfo.Name.ToLower() == "lib")
            {
                return;
            }
            
            var cssFiles = Directory.GetFiles(directoryPath, "*.css");

            foreach (var cssFile in cssFiles)
            {
                _fileSystem.File.Delete(cssFile);
                Console.WriteLine($"Deleted: {cssFile}");
            }

            var subdirectories = _fileSystem.Directory.GetDirectories(directoryPath);

            foreach (var subdirectory in subdirectories)
            {
                DeleteCssFiles(subdirectory);
            }
        }
    }
}

