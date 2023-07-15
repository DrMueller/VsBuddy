using System;
using StructureMap;

namespace VsBuddy.Infrastructure.DependencyInjection
{
    public static class ApplicationServiceLocator
    {
        private static readonly Lazy<Container> _lazyLocator = new Lazy<Container>(
            () =>
            {
                var container = new Container();
                container.Configure(
                    cfg =>
                    {
                        cfg.Scan(
                            scanner =>
                            {
                                scanner.AssemblyContainingType<ApplicationRegistry>();
                                scanner.LookForRegistries();
                            });
                    });

                return container;
            });

        public static T GetService<T>()
        {
            return _lazyLocator.Value.GetInstance<T>();
        }
    }
}