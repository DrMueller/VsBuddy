using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace VsBuddy
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(VsBuddyPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class VsBuddyPackage : AsyncPackage
    {
        /// <summary>
        /// VsBuddyPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "265c39b3-4208-464f-a6ac-9da51683a085";

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await VsBuddy.Areas.CreateUnitTests.CreateUnitTestsCommand.InitializeAsync(this);
        }

        #endregion
    }
}
