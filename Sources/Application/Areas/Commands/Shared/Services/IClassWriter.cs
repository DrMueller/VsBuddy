using VsBuddy.Areas.Commands.Shared.Models;

namespace VsBuddy.Areas.Commands.Shared.Services
{
    public interface IClassWriter
    {
        void Write(string basePath, params ClassMetadata[] classes);
    }
}