using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Algolia.HttpClients.Abstract;
using Soenneker.Algolia.OpenApiClientUtil.Abstract;
using Soenneker.Algolia.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Algolia.OpenApiClientUtil;

///<inheritdoc cref="IAlgoliaOpenApiClientUtil"/>
public sealed class AlgoliaOpenApiClientUtil : IAlgoliaOpenApiClientUtil
{
    private readonly AsyncSingleton<AlgoliaOpenApiClient> _client;

    public AlgoliaOpenApiClientUtil(IAlgoliaOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<AlgoliaOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Algolia:ApiKey");
            string authHeaderValueTemplate = configuration["Algolia:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerValue: authHeaderValue), httpClient: httpClient);

            return new AlgoliaOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<AlgoliaOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
