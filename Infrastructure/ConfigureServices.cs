using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Core.Interfaces;



namespace Infrastructure
{
   
        public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddDbContext<StoreContext>
                (options => options.UseSqlite(configuration.GetSection("ConnectionStrings:DefaultConnection").Value));
            services.AddScoped<ApplicationDbContextInitialiser>();

        

            return services;
        }
    }
    
}