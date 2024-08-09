using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VsBuddy.Areas.Translations.CreateResx.Orchestration;
using VsBuddy.Infrastructure.VisualStudio.Context;

namespace VsBuddy.Areas.Translations.CreateResx
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class CreateResxCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b7895518-1080-4487-b0cc-dfdd70eb0e3e");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static CreateResxCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateResxCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private CreateResxCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in CreateResxCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CreateResxCommand(package, commandService);
        }

        /// <summary>
        ///     This function is the callback used to execute the command when the menu item is clicked.
        ///     See the constructor to see how the menu item is associated with this function using
        ///     OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void Execute(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var dte = (DTE)await _package.GetServiceAsync(typeof(DTE));

            if (dte == null)
            {
                throw new ArgumentNullException(nameof(dte));
            }

            var selectedItems = dte.SelectedItems;

            if (selectedItems == null)
            {
                return;
            }

            foreach (SelectedItem selectedItem in selectedItems)
            {
                if (!(selectedItem.ProjectItem is ProjectItem projectItem))
                {
                    continue;
                }

                var filePath = projectItem.FileNames[0];

                VsContext
                    .Execute(container =>
                    {
                        var unitTestClassWriter = container.GetInstance<ICreateResxOrchestrator>();
                        unitTestClassWriter.Execute(filePath);
                    }, _package);

                break;
            }
        }
    }
}