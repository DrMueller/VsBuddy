using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace VsBuddy.Infrastructure.VisualStudio.Helpers
{
    public static class SelectedItemsHelper
    {
        public static async Task<IReadOnlyCollection<ProjectItem>> GetSelectedProjectItemsAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var dte = (DTE)await package.GetServiceAsync(typeof(DTE));

            if (dte == null)
            {
                throw new ArgumentNullException(nameof(dte));
            }

            var result = new List<ProjectItem>();

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.ProjectItem is ProjectItem projectItem)
                {
                    result.Add(projectItem);
                }
            }

            return result;
        }
    }
}