using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

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

    public static async Task<T> LastCapPublishedMessage<T>(
        this DbContext context,
        string routingQueue
    )
    {
        var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText =
            $"Select top 1 Content FROM [cap].Published Where [Name] = '{routingQueue}' order by Id desc";

        using var reader = await command.ExecuteReaderAsync();
        var table = new DataTable();
        table.Load(reader);
        var content = table.Rows[0]["Content"].ToString();

        var messageBody = JsonDocument
            .Parse(content)
            .RootElement.GetProperty("Value")
            .Deserialize<T>();
        return messageBody ?? throw new InvalidOperationException();
    }
}
