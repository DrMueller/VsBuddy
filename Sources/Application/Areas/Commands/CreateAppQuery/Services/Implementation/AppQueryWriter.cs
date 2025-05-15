using VsBuddy.Areas.Commands.CreateAppQuery.Models;
using VsBuddy.Areas.Commands.Shared.Models;
using VsBuddy.Areas.Commands.Shared.Services;

namespace VsBuddy.Areas.Commands.CreateAppQuery.Services.Implementation
{
    public class AppQueryWriter : IAppQueryWriter
    {
        private readonly IClassWriter _writer;

        public AppQueryWriter(IClassWriter writer)
        {
            _writer = writer;
        }

        public void CreateAppQuery(string folderName)
        {
            var appFolder = new AppFolder(folderName);

            var queryResultClass = new AppQueryResultClass(appFolder.Namespace);
            var queryClass = new AppQueryClass(appFolder.Namespace, queryResultClass.ClassName);
            var queryHandlerClass = new AppQueryHandlerClass(
                appFolder.Namespace,
                queryResultClass.ClassName,
                queryClass.ClassName);

            _writer.Write(appFolder.Path, queryClass, queryHandlerClass, queryResultClass);
        }
    }
}