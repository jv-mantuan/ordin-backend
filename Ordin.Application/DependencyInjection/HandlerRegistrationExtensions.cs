using Ordin.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ordin.Application.DependencyInjection
{
    public sealed class ApplicationAssemblyMarker { }

    public static class HandlerRegistrationExtensions
    {
        public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
        {
            RegisterHandlers(services, typeof(ICommandHandler<>), typeof(ICommandHandler<,>), typeof(IQueryHandler<,>));
            return services;
        }

        private static void RegisterHandlers(IServiceCollection services, params Type[] openHandlerTypes)
        {
            var types = typeof(ApplicationAssemblyMarker)
                .Assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericTypeDefinition);

            foreach (var implementationType in types)
            {
                foreach (var @interface in implementationType.GetInterfaces())
                {
                    if (!@interface.IsGenericType)
                        continue;

                    if (services.Any(d => d.ServiceType == @interface))
                    {
                        throw new InvalidOperationException(
                            $"Multiple handlers registered for {@interface.FullName}");
                    }

                    var genericDefinition = @interface.GetGenericTypeDefinition();

                    if (!openHandlerTypes.Contains(genericDefinition))
                        continue;

                    services.AddScoped(@interface, implementationType);
                }
            }
        }
    }
}
