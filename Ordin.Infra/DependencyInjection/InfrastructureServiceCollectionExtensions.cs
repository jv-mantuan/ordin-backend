using Ordin.Application.Interfaces;
using Ordin.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Ordin.Infra.DependencyInjection
{
    public sealed class InfrastructureAssemblyMarker { }

    public static class InfrastructureServiceCollectionExtensions
    {
        /// <summary>
        /// Registers repository services with the dependency injection container by mapping each repository interface
        /// to its corresponding implementation found in the specified assembly.
        /// </summary>
        /// <remarks>This method scans the provided assembly for non-abstract, non-generic classes whose
        /// names end with 'Repository' and registers them as scoped services for each implemented repository interface,
        /// except for the base repository interface. This ensures that each repository interface is registered only
        /// once.</remarks>
        /// <param name="services">The service collection to which the repository implementations will be added.</param>
        /// <param name="assembly">The assembly to scan for repository implementation types to register.</param>
        /// <exception cref="InvalidOperationException">Thrown if a repository interface is already registered in the service collection.</exception>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            RegisterServices(services, typeof(InfrastructureAssemblyMarker).Assembly);
            return services;
        }

        private static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            var types = typeof(InfrastructureAssemblyMarker).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository", StringComparison.Ordinal)
                    && !t.IsGenericTypeDefinition);

            foreach (var implementationType in types)
            {
                var interfaces = implementationType.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Repository", StringComparison.Ordinal) && i != typeof(IBaseRepository<>))
                    .ToList();

                foreach (var serviceType in interfaces)
                {
                    if (services.Any(d => d.ServiceType == serviceType))
                    {
                        throw new InvalidOperationException(
                            $"DI duplicado para {serviceType.FullName}.");
                    }

                    services.AddScoped(serviceType, implementationType);
                }
            }
        }
    }
}