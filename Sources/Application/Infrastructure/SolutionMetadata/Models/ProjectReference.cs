using System.IO;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class ProjectReference
    {
        public string AssemblyName
        {
            get
            {
                var fileName = Path.GetFileName(RelativePath);

                return fileName.Replace(".csproj", string.Empty);
            }
        }

        private string RelativePath { get; }

        public ProjectReference(string relativePath)
        {
            RelativePath = relativePath;
        }
    }
}