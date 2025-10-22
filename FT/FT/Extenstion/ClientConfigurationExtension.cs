using FT.Client.Helper;
using FT.Core.Security.ClientInfo;
using Microsoft.OpenApi.Models;


namespace FT.Client.Extenstion
{
    public static class ClientConfigurationExtension
    {
        public static void ConfigureClientStartupServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            ConfigureSwagger(services);
            ConfigureClientRHelper(services);
        }
       
        private static void ConfigureSwagger(IServiceCollection services)
        {

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
            //    c.AddSecurityDefinition("TenantId", new OpenApiSecurityScheme
            //    {
            //        Name = "TenantId",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "TenantId",
            //        In = ParameterLocation.Header,
            //        Description = "TenantId"
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //          new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "TenantId"
            //                }
            //            },
            //            new string[] {}

            //    }
            //});


            //    //* Cookie authentication enabled so this code is commented
            //    //* uncomment if required authentication from token

            //    //https://thecodebuzz.com/jwt-authorization-token-swagger-open-api-asp-net-core-3-0/
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Authorization header using the Bearer scheme."
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //          new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}

            //    }
            //});



                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "REST services for Finanacial Tracker.",
                    Version = "v1",
                    Description = "Through this API you can access FT services",
                    //Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    //{
                    //    Email = "dev@rigonepal.com",
                    //    Name = "Rigo Technologies",
                    //    Url = new Uri("https://rigonepal.com/")
                    //}
                });
                //var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                //c.IncludeXmlComments(xmlCommentsFullPath);
            });


        }

        public static void ConfigureClientRHelper(this IServiceCollection services)
        {
            services.AddSingleton<IClientInfoProvider, HttpContextAccessorProvider>();
        }
    }
}
