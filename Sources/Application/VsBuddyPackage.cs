﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using VsBuddy.Areas.ClearCss;
using VsBuddy.Areas.CreateBlazorUnitTests;
using VsBuddy.Areas.CreateResx;
using VsBuddy.Areas.CreateUnitTests;

namespace VsBuddy
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class VsBuddyPackage : AsyncPackage
    {
        public const string PackageGuidString = "265c39b3-4208-464f-a6ac-9da51683a085";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await CreateUnitTestsCommand.InitializeAsync(this);
            await CreateResxCommand.InitializeAsync(this);
            await ClearCssCommand.InitializeAsync(this);
            await CreateBlazorUnitTestsCommand.InitializeAsync(this);
        }
    }
}