using Soenneker.Algolia.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Algolia.OpenApiClientUtil.Abstract;

/// <summary>
/// Creates and caches an authenticated <see cref="AlgoliaOpenApiClient"/>.
/// </summary>
public interface IAlgoliaOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the cached client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel initial client creation.</param>
    /// <returns>The cached generated client.</returns>
    ValueTask<AlgoliaOpenApiClient> Get(CancellationToken cancellationToken = default);
}
