using FluentValidation;
using DS.Handlers.Abstract;
using DS.Infrastructure.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DS.Handlers
{
    public class HandlersDiConfig
    {
        public static void RegisterAllDependencies(IServiceCollection services)
        {
            RegisterHandlers(services);
            RegisterFactories(services);
            RegisterValidators(services);
        }

        public static void RegisterHandlers(IServiceCollection services)
        {
            GetAllHandlers()
                .ForEach(handler => RegisterScopedHandler(services, handler));
        }

        public static void RegisterFactories(IServiceCollection services)
        {
            DependencyRegistration.RegisterScopedInterfaces(services, typeof(HandlersDiConfig), @namespace: "DS.Handlers.Strategies.Factories", componentNameSuffix: "Factory");
        }

        public static void RegisterValidators(IServiceCollection services)
        {
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(HandlersDiConfig).GetTypeInfo().Assembly)
                .ForEach(pair =>
                {
                    services.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
                });
        }

        private static List<Type> GetAllHandlers()
        {
            var assembly = typeof(HandlersDiConfig).GetTypeInfo().Assembly;
            return assembly.GetTypes()
                    .Where(type => type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract)
                    .Where(IsHandlerBase)
                    .ToList();
        }

        private static bool IsHandlerBase(Type type)
        {
            return (type.GetTypeInfo()?.BaseType != null
                    && (type.GetTypeInfo().BaseType.Name == "HandlerBase`2" || IsHandlerBase(type.GetTypeInfo().BaseType)));
        }

        private static void RegisterScopedHandler(IServiceCollection services, Type handler)
        {
            var (requestType, responseType) = GetRequestAndResponseTypes(handler);
            services.AddScoped(typeof(HandlerBase<,>).MakeGenericType(requestType, responseType), handler);
        }

        private static (Type RequestType, Type ResponseType) GetRequestAndResponseTypes(Type handler)
        {
            var generics = handler.GetTypeInfo().BaseType.GetGenericArguments();
            return (generics[0], generics[1]);
        }
    }
}
