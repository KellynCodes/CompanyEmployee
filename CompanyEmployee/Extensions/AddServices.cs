using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Implementations;
using Repository.Interfaces;
using Service.Contracts;
using Services.Implementations;

namespace CompanyEmployee.Extensions
{
    public static class AddService
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddDbContext<CompEmpDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }
    }
}
