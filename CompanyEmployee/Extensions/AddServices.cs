using CompanyEmployee.Formaters;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Implementations;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using Services.Implementations;

namespace CompanyEmployee.Extensions
{
    public static class AddService
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddTransient<ILoggerManager, LoggerManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(Program));

            services.AddDbContext<CompEmpDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }

        public static void AddDefaultController(this IServiceCollection services)
        {
            services.AddControllers(config => {
                config.RespectBrowserAcceptHeader = true;
                //return 406 if no reponse type was selected
               //config.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter()
            .AddApplicationPart(typeof(Presentation.ReferencedAssembly).Assembly);
        }
    }
}
