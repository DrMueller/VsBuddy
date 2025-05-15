using System.IO;
using VsBuddy.Areas.Commands.Shared.Models;

namespace VsBuddy.Areas.Commands.Shared.Services.Implementation
{
    public class ClassWriter : IClassWriter
    {
        public void Write(string basePath, params ClassMetadata[] classes)
        {
            foreach (var classMetadata in classes)
            {
                var filePath = Path.Combine(basePath, $"{classMetadata.CsFileName}");
                File.WriteAllText(filePath, classMetadata.Content);
            }
        }
    }
}