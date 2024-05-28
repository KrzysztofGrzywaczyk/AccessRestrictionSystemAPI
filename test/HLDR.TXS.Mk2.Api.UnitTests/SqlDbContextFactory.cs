using AccessControlSystem.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccessControlSystem.Api.UnitTests;

public static class SqlDbContextFactory
{
    public static SqlDbContext Create()
    {
        var options = new DbContextOptionsBuilder<SqlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .Options;

        return new SqlDbContext(options);
    }
}