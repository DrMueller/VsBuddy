using VsBuddy.Areas.Commands.CreateAppCommand.Models;
using VsBuddy.Areas.Commands.Shared.Models;
using VsBuddy.Areas.Commands.Shared.Services;

namespace VsBuddy.Areas.Commands.CreateAppCommand.Services.Implementation
{
    public class AppCommandWriter : IAppCommandWriter
    {
        private readonly IClassWriter _writer;

        public AppCommandWriter(IClassWriter writer)
        {
            _writer = writer;
        }

        public void CreateAppCommand(string folderName)
        {
            var appFolder = new AppFolder(folderName);

            var commandClass = new AppCommandClass(appFolder.Namespace);
            var commandHandlerClass = new AppCommandHandlerClass(appFolder.Namespace, commandClass.ClassName);

            _writer.Write(appFolder.Path, commandClass, commandHandlerClass);
        }
    }
}