using Soenneker.Algolia.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Algolia.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IAlgoliaOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    ValueTask<AlgoliaOpenApiClient> Get(CancellationToken cancellationToken = default);
}
