using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private readonly string _connectionString;

        public DatabaseContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DatabaseContextFactory()
        {
            //TODO: This is a problem. We need a parameterless constructor for the code to work... so how do we inject IConfig?
            _connectionString =  "Server=localhost;Database=Api;Trusted_Connection=True;";
        }
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
