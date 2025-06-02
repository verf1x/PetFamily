﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Entities;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private new const string Database = nameof(Database);
    
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    
    public DbSet<PetSpecies> Species => Set<PetSpecies>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Database));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory()
        => LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
}