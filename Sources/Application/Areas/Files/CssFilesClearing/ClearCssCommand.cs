using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsBuddy.Areas.Files.CssFilesClearing.Services;
using VsBuddy.Infrastructure.VisualStudio.Context;

namespace VsBuddy.Areas.Files.CssFilesClearing
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class ClearCssCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("77208b7c-bee7-4d44-b0d5-779d48bcdcfe");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static ClearCssCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => _package;

        private ClearCssCommand(AsyncPackage package, OleMenuCommandService commandService)
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
            // Switch to the main thread - the call to AddCommand in ClearCssCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ClearCssCommand(package, commandService);
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
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

#pragma warning disable VSSDK006 // Check services exist
            var dte2 = await _package.GetServiceAsync(typeof(SDTE)) as DTE2;
#pragma warning restore VSSDK006 // Check services exist
            // ReSharper disable once PossibleNullReferenceException
            var solutionFileName = dte2.Solution.FileName;

            VsContext
                .Execute(container =>
                {
                    var clearer = container.GetInstance<ICssClearer>();
                    clearer.Clear(solutionFileName);
                }, _package);
        }
    }
}