namespace AspnetTemplate.Tests;

public class ProgramTests : IClassFixture<AppFixture>
{
    private readonly AppFixture _factory;

    public ProgramTests(AppFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_HealthCheck_ShouldReturnHealthy()
    {
        var client = _factory.CreateClient();

        var result = await client.GetStringAsync("health");

        Assert.Equal("Healthy", result);
    }
}
