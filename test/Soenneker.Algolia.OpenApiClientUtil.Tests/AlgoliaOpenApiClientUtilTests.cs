using Soenneker.Algolia.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Algolia.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class AlgoliaOpenApiClientUtilTests : HostedUnitTest
{
    private readonly IAlgoliaOpenApiClientUtil _openapiclientutil;

    public AlgoliaOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<IAlgoliaOpenApiClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
