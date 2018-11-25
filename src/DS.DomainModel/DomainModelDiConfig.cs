using DS.Infrastructure.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace DS.DomainModel
{
    public static class DomainModelDiConfig
    {
        public static void RegisterFactories(IServiceCollection services)
        {
            DependencyRegistration.RegisterScopedInterfaces(services, typeof(DomainModelDiConfig), @namespace: "DS.DomainModel.Entities", componentNameSuffix: "Factory");
        }
    }
}
