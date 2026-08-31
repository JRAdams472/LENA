using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FluentValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidatorsFromAssembly(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var validatorRegistrations = assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => new { Interface = i, Implementation = t }))
                .ToList();

            foreach (var registration in validatorRegistrations)
            {
                services.Add(new ServiceDescriptor(registration.Interface, registration.Implementation, lifetime));
            }

            return services;
        }
    }
}
