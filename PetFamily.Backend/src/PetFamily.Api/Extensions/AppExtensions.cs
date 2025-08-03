using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Api.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
    
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        await writeDbContext.Database.MigrateAsync();
    }
}