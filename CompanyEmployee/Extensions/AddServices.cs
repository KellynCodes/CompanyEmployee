using AspNetCoreRateLimit;
using CompanyEmployee.Formaters;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilter;
using Presentation.Utility;
using Repository.Context;
using Repository.Dtos;
using Repository.Implementations;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using Services.Implementations;
using Services.Interfaces;

namespace CompanyEmployee.Extensions
{
    public static class AddService
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddTransient<ILoggerManager, LoggerManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<IEmployeeLinks, EmployeeLinks>();
            services.AddScoped<ValidationFilterAttribute>();

            services.AddAutoMapper(typeof(Program));
            services.AddDbContext<CompEmpDbContext>(opts =>
            opts.UseSqlServer(configuration["ConnectionStrings:sqlConnection"]));
        }

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
   {
       options.AddPolicy("CorsPolicy", builder =>
       builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader()
       .WithExposedHeaders("X-Pagination"));
   });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //if you want to use api query versioning
                //opt.ApiVersionReader = new QueryStringApiVersionReader("api-version");

                //If we have a lot of versions of a single controller, we can assign these versions in the configuration instead:
                // opt.Conventions.Controller<CompaniesController>().HasApiVersion(new ApiVersion(1, 0));
                // opt.Conventions.Controller<CompaniesV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));
                //after these configuration here you are surposed to remove the [apiVersion] attribute in the controllers configured here...

            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();
        //install Marvin.Cache.Headers
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(
            (expirationOpt) =>
            {
                expirationOpt.MaxAge = 65;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },

            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
             new RateLimitRule
             {
                 Endpoint = "*",
                 Limit = 3,
                 Period = "5m"
             }};
            services.Configure<IpRateLimitOptions>(opt =>
                        {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CompEmpDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddConnection(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<CompEmpDbContext>(dbOption =>
            {
                var ConnectionString = builder.Configuration["ConnectionStrings:sqlConnection"];
                dbOption.UseSqlServer(ConnectionString);
            });
        }


    }
}
