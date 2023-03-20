using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Repository.Context;
using Repository;


namespace CompanyEmployee.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<CompEmpDbContext>
    {
        public CompEmpDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new DbContextOptionsBuilder<CompEmpDbContext>()
   .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
   b => b.MigrationsAssembly("CompanyEmployee"));
            return new CompEmpDbContext(builder.Options);

        }
    }

}
