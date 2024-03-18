using AspnetTemplate.Core.Database;
using Microsoft.EntityFrameworkCore;
using System.CommandLine;

var connectionStringOption = new Option<string>(
    name: "--connection-string",
    description: "Connection string to the database."
)
{
    IsRequired = true
};

var migrateCommand = new Command("migrate", "Run migrations.") { connectionStringOption };
var rootCommand = new RootCommand("Tools for AspnetTemplate.");
rootCommand.AddCommand(migrateCommand);

migrateCommand.SetHandler(
    async (connectionString) =>
    {
        Console.WriteLine("Starting migrations...");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
        Console.WriteLine("Done!");
    },
    connectionStringOption
);

return await rootCommand.InvokeAsync(args);
