using FT.EntityFramework.EntityFramework.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.DependencyResolution
{
    public static class EntityFrameworkConfiguration
    {
        public static void ConfigureEfStartup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((services, options) =>
            {
                var connectionString = configuration.GetConnectionString("DbConnectionString");



                options.UseSqlServer(connectionString);
            });
        }
    }
}
