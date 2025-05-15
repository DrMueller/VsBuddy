using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using VsBuddy.Areas.Maps.CreateMap.Services;
using VsBuddy.Infrastructure.VisualStudio.Context;
using VsBuddy.Infrastructure.VisualStudio.Helpers;
using Task = System.Threading.Tasks.Task;

namespace VsBuddy.Areas.Maps.CreateMap
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class CreateMapCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("8c71ff22-0433-45de-a393-3a4aff4bda10");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static CreateMapCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateMapCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private CreateMapCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
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
            // Switch to the main thread - the call to AddCommand in CreateMapCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CreateMapCommand(package, commandService);
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void Execute(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var selectedItems = await SelectedItemsHelper.GetSelectedProjectItemsAsync(_package);

            foreach (var projItem in selectedItems)
            {
                var filePath = projItem.FileNames[0];

                VsContext
                    .Execute(container =>
                    {
                        var mapWriter = container.GetInstance<IMapWriter>();
                        mapWriter.CreateMap(filePath);
                    }, _package);
            }
        }
    }
}