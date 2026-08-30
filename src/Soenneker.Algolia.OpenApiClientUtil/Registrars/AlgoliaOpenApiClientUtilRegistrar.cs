using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Algolia.HttpClients.Registrars;
using Soenneker.Algolia.OpenApiClientUtil.Abstract;

namespace Soenneker.Algolia.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class AlgoliaOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IAlgoliaOpenApiClientUtil"/> as a singleton service backed by the singleton HTTP-client provider.
    /// </summary>
    public static IServiceCollection AddAlgoliaOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddAlgoliaOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IAlgoliaOpenApiClientUtil, AlgoliaOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IAlgoliaOpenApiClientUtil"/> as a scoped service backed by the singleton HTTP-client provider.
    /// </summary>
    public static IServiceCollection AddAlgoliaOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddAlgoliaOpenApiHttpClientAsSingleton()
                .TryAddScoped<IAlgoliaOpenApiClientUtil, AlgoliaOpenApiClientUtil>();

        return services;
    }
}
