using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DS.Infrastructure.Dependency
{
    public static class DependencyRegistration
    {
        public static void RegisterScopedInterfaces(IServiceCollection services, Type type, string @namespace, string componentNameSuffix)
        {
            GetAllComponents(type, @namespace, componentNameSuffix)
                .ForEach(component => RegisterScopedInterface(services, component, componentNameSuffix));
        }

        private static List<Type> GetAllComponents(Type type, string @namespace, string componentNameSuffix)
        {
            var assembly = type.GetTypeInfo().Assembly;
            return assembly.GetTypes()
                    .Where(IsNotAnonymousType)
                    .Where(IsNotInterface)
                    .Where(x => x.Namespace.StartsWith(@namespace) && x.Name.EndsWith(componentNameSuffix))
                    .ToList();
        }

        private static bool IsNotAnonymousType(Type type)
        {
            return !(type.Name.Contains("AnonymousType")
                    && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")));
        }

        private static bool IsNotInterface(Type type) => !type.IsInterface;

        private static void RegisterScopedInterface(IServiceCollection services, Type componentToRegister, string componentNameSuffix)
        {
            var interfaceType = componentToRegister.GetInterfaces().Single(@interface => @interface.Name.EndsWith(componentNameSuffix));
            services.AddScoped(interfaceType, componentToRegister);
        }
    }
}
