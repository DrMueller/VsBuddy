using VsBuddy.Infrastructure.VisualStudio.Messaging.Models;

namespace VsBuddy.Infrastructure.VisualStudio.Messaging.Services
{
    public interface IMessageService
    {
        void ShowMessage(string message, MessageType messageType);
    }
}