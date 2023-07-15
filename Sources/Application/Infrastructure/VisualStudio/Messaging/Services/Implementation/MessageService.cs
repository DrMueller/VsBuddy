using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;
using VsBuddy.Infrastructure.VisualStudio.ServiceProviders;

namespace VsBuddy.Infrastructure.VisualStudio.Messaging.Services.Implementation
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message, MessageType messageType)
        {
            VsShellUtilities.ShowMessageBox(
                ServiceProviderSingleton.Instance,
                message,
                "VS Buddy",
                MapIcon(messageType),
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private static OLEMSGICON MapIcon(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Critical:
                    return OLEMSGICON.OLEMSGICON_CRITICAL;
                case MessageType.Warning:
                    return OLEMSGICON.OLEMSGICON_WARNING;
                default:
                    return OLEMSGICON.OLEMSGICON_INFO;
            }
        }
    }
}