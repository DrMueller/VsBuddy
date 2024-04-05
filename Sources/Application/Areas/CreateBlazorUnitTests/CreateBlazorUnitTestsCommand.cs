using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VsBuddy.Areas.CreateBlazorUnitTests.Orchestration.Services;
using VsBuddy.Infrastructure.VisualStudio.Context;
using Task = System.Threading.Tasks.Task;

namespace VsBuddy.Areas.CreateBlazorUnitTests
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class CreateBlazorUnitTestsCommand
    {
        public const int CommandId = 256;

        public static readonly Guid CommandSet = new Guid("fa9ae8ce-5cfd-4817-8146-d86f09a2f8d3");

        private readonly AsyncPackage _package;

        public static CreateBlazorUnitTestsCommand Instance
        {
            get;
            private set;
        }

        private IAsyncServiceProvider ServiceProvider => _package;

        private CreateBlazorUnitTestsCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(Execute, menuCommandID);

            menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;

            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CreateBlazorUnitTestsCommand(package, commandService);
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void Execute(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var selectedItems = await GetSelectedItemsAsync();

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
                        var unitTestClassWriter = container.GetInstance<IBlazorUnitTestClassWriter>();
                        unitTestClassWriter.CreateTestClass(filePath);
                    }, _package);

                break;
            }
        }

        private async Task<SelectedItems> GetSelectedItemsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var dte = (DTE)await _package.GetServiceAsync(typeof(DTE));

            if (dte == null)
            {
                throw new ArgumentNullException(nameof(dte));
            }

            return dte.SelectedItems;
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var exts = new List<string> { "blazor", "blazor.cs" };

            if (sender is OleMenuCommand command)
            {
                command.Visible = false; // Default to not visible

                var selectedItems = await GetSelectedItemsAsync();

                if (selectedItems == null)
                {
                    return;
                }

                foreach (var item in selectedItems)
                {
                    if (!(item is ProjectItem projectItem))
                    {
                        continue;
                    }

                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                    if (exts.Any(ext => projectItem.Name.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        command.Visible = true;
                    }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                }
            }
        }
    }
}