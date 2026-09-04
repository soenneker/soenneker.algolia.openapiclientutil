using System;
using System.Collections.Generic;
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

/// <inheritdoc cref="IAlgoliaOpenApiClientUtil" />
public sealed class AlgoliaOpenApiClientUtil : IAlgoliaOpenApiClientUtil
{
    private readonly AsyncSingleton<AlgoliaOpenApiClient> _client;

    public AlgoliaOpenApiClientUtil(IAlgoliaOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<AlgoliaOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Algolia:ApiKey");
            var applicationId = configuration.GetValueStrict<string>("Algolia:ApplicationId");
            string authHeaderName = configuration["Algolia:AuthHeaderName"] ?? "X-Algolia-API-Key";
            string authHeaderValueTemplate = configuration["Algolia:AuthHeaderValueTemplate"] ?? "{token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var additionalHeaders = new Dictionary<string, string>
            {
                ["X-Algolia-Application-Id"] = applicationId
            };
            var authenticationProvider = new GenericAuthenticationProvider(authHeaderName, authHeaderValue, additionalHeaders);
            var requestAdapter = new HttpClientRequestAdapter(authenticationProvider, httpClient: httpClient);

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
