[![](https://img.shields.io/nuget/v/soenneker.algolia.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.algolia.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.algolia.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.algolia.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.algolia.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.algolia.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.algolia.openapiclientutil/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.algolia.openapiclientutil/actions/workflows/codeql.yml)

# Soenneker.Algolia.OpenApiClientUtil

Creates and caches an authenticated `AlgoliaOpenApiClient` for dependency-injected applications.

## Installation

```bash
dotnet add package Soenneker.Algolia.OpenApiClientUtil
```

## Configuration

```json
{
  "Algolia": {
    "ApplicationId": "your-application-id",
    "ApiKey": "your-api-key",
    "ClientBaseUrl": "https://analytics.eu.algolia.com"
  }
}
```

All three settings are required. Choose a concrete `ClientBaseUrl` for the Algolia product and region you intend to call.

Authentication defaults to the `X-Algolia-Application-Id` and `X-Algolia-API-Key` headers. `Algolia:AuthHeaderName` and `Algolia:AuthHeaderValueTemplate` can override the API-key header when a service requires a different format; `{token}` is replaced with `ApiKey`.

## Registration

```csharp
using Soenneker.Algolia.OpenApiClientUtil.Registrars;

services.AddAlgoliaOpenApiClientUtilAsScoped();
```

The scoped utility uses a singleton HTTP-client provider, so ending a scope does not remove the shared cached `HttpClient`. Use `AddAlgoliaOpenApiClientUtilAsSingleton()` when the generated client should also be shared application-wide.

## Usage

```csharp
using Soenneker.Algolia.OpenApiClient;
using Soenneker.Algolia.OpenApiClient.Models;
using Soenneker.Algolia.OpenApiClientUtil.Abstract;

public sealed class AlgoliaStatusService
{
    private readonly IAlgoliaOpenApiClientUtil _clientUtil;

    public AlgoliaStatusService(IAlgoliaOpenApiClientUtil clientUtil)
    {
        _clientUtil = clientUtil;
    }

    public async Task<StatusResponseResponse?> GetStatus(CancellationToken cancellationToken = default)
    {
        AlgoliaOpenApiClient client = await _clientUtil.Get(cancellationToken);
        return await client.Monitoring.One.Status.GetAsync(cancellationToken: cancellationToken);
    }
}
```

## Behavior

- `Get()` lazily creates one generated client per utility instance and returns it on subsequent calls.
- The client has one configured base URL. Create separate utility registrations or clients when calling Algolia products on different hosts.
- Authentication credentials are added only to HTTPS requests and are pinned to the first request host.
- Configuration is read during initial creation and does not rebuild an already cached client.
- Let the dependency-injection container dispose resolved utilities.
