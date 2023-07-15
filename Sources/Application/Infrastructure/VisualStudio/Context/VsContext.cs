using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using VsBuddy.Infrastructure.DependencyInjection;
using VsBuddy.Infrastructure.VisualStudio.Messaging.Services;
using VsBuddy.Infrastructure.VisualStudio.ServiceProviders;

namespace VsBuddy.Infrastructure.VisualStudio.Context
{
    public static class VsContext
    {
        public static void Execute(Action<IContainer> act, IServiceProvider provider)
        {
            ServiceProviderSingleton.Initialize(provider);

            try
            {
                act(ApplicationServiceLocator.Instance);
            }
            catch (Exception ex)
            {
                var messageService = ApplicationServiceLocator.Instance.GetInstance<IMessageService>();

                messageService.ShowMessage(ex.Message);
            }
        }
    }
}
