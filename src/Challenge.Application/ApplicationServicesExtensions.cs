using System;
using System.Linq;
using System.Reflection;
using Challenge.Common;
using Challenge.Common.Commands;
using Challenge.Common.Events;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Action<Type, Type, ServiceLifetime> configureInterceptor = null)
        {
            DomainEvents.RegisterHandlers(Assembly.GetExecutingAssembly(), services);

            services.AddSingleton<IDomainEvents, DomainEvents>();
            services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            if (configureInterceptor != null)
            {
                var aggregateRootTypes = typeof(AggregateRoot<>).Assembly.GetTypes().Where(x => x.BaseType == typeof(AggregateRoot<string>)).ToList();
                foreach (var type in aggregateRootTypes)
                {
                    configureInterceptor(typeof(ICrudService<>).MakeGenericType(type), typeof(CrudService<>).MakeGenericType(type), ServiceLifetime.Scoped);
                }
            }

            return services;
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            services.AddScoped<Dispatcher>();

            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in assemblyTypes)
            {
                var handlerInterfaces = type.GetInterfaces()
                   .Where(Utils.IsHandlerInterface)
                   .ToList();

                if (!handlerInterfaces.Any())
                {
                    continue;
                }

                var handlerFactory = new HandlerFactory(type);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }

            return services;
        }
    }
}