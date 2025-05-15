using System;
using System.Linq;

namespace VsBuddy.Areas.Commands.Shared.Models
{
    public class AppFolder
    {
        public string Namespace
        {
            get
            {
                var parts = Path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.All(f => f != "Application"))
                {
                    return string.Join(".", parts);
                }

                var applicationIndex = Array.IndexOf(parts, "Application");
                var namespaceParts = parts.Skip(applicationIndex + 1).ToArray();

                return string.Join(".", namespaceParts);
            }
        }

        public string Path { get; }

        public AppFolder(string folderPath)
        {
            Path = folderPath;
        }
    }
}