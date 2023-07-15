using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsBuddy.Infrastructure.VisualStudio.ServiceProviders;

namespace VsBuddy.Infrastructure.VisualStudio.Messaging.Services.Implementation
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            VsShellUtilities.ShowMessageBox(
                ServiceProviderSingleton.Instance,
                message,
                "VS Buddy",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
