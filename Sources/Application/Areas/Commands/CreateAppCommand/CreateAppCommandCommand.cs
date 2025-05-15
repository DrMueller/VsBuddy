using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using VsBuddy.Areas.Commands.CreateAppCommand.Services;
using VsBuddy.Infrastructure.VisualStudio.Context;
using VsBuddy.Infrastructure.VisualStudio.Helpers;
using Task = System.Threading.Tasks.Task;

namespace VsBuddy.Areas.Commands.CreateAppCommand
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class CreateAppCommandCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("11a3b512-be12-4c23-b6d1-de8e4dcf8948");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static CreateAppCommandCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateAppCommandCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private CreateAppCommandCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in CreateAppCommandCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CreateAppCommandCommand(package, commandService);
        }

        private async void Execute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var selectedItems = await SelectedItemsHelper.GetSelectedProjectItemsAsync(package);

            foreach (var item in selectedItems)
            {
                var filePath = item.FileNames[0];
                VsContext
                    .Execute(container =>
                    {
                        var appCommandWriter = container.GetInstance<IAppCommandWriter>();
                        appCommandWriter.CreateAppCommand(filePath);
                    }, package);
            }
        }
    }
}