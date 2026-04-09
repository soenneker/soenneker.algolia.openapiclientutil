using Soenneker.Algolia.OpenApiClientUtil.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Algolia.OpenApiClientUtil.Tests;

[Collection("Collection")]
public sealed class AlgoliaOpenApiClientUtilTests : FixturedUnitTest
{
    private readonly IAlgoliaOpenApiClientUtil _openapiclientutil;

    public AlgoliaOpenApiClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _openapiclientutil = Resolve<IAlgoliaOpenApiClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
