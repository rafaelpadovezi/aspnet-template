namespace AspnetTemplate.Tests.Support;

public static class Extensions
{
    public static async Task FailIfNotSuccess(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Request return an Error.\n{response.StatusCode}\n{errorContent}");
        }
    }
}
