using System;

namespace VsBuddy.Infrastructure.VisualStudio.ServiceProviders
{
    public static class ServiceProviderSingleton
    {
        public static IServiceProvider Instance { get; private set; }

        public static void Initialize(IServiceProvider instance)
        {
            Instance = instance;
        }
    }
}