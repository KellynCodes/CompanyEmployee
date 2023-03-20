using Contracts;
using Repository;

namespace CompanyEmployee.Extensions
{
    public static class ServiceManager
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
services.AddScoped<IRepositoryManager, RepositoryManager>();


    }
}
