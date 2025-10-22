using FT.Client.IdentityConfig.ConfigMiddleWare;
using FT.Core.Security.User;
using FT.DependencyResolution;
using FT.EntityFramework.EntityFramework.DbContext;
using FT.EntityFramework.EntityFramework.Security.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace FT.Client.IdentityConfig
{
    public class AuthenticationStartup
    {

        public void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {

            var identitySection = configuration.GetSection("IdentityOptions");
            services.Configure<FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig>(identitySection);

            EntityFrameworkConfiguration.ConfigureEfStartup(services, configuration);

            var identityOption = identitySection.Get<FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig>();
            ConfigureIdentityOptions(services,identityOption);
            ConfigureJwtBearer(services, configuration, identityOption);
        }


        private void ConfigureIdentityOptions(IServiceCollection services, FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig identityOption)
        {
            services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<SignInManager<ApplicationIdentityUser>, ApplicationSignInManager>();
            services.AddScoped<UserManager<ApplicationIdentityUser>, ApplicationUserManagementService>();
            //services.AddScoped<RoleManager<ApplicationIdentityRole>, ApplicationRoleManager>();
            services.AddScoped<IApplicationUserManagementService, ApplicationUserManagementService>();
            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            //services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = identityOption.Password.RequireDigit;
                options.Password.RequiredLength = identityOption.Password.RequiredLength;
                options.Password.RequireNonAlphanumeric = identityOption.Password.RequireNonAlphanumeric;
                options.Password.RequireUppercase = identityOption.Password.RequireUppercase;
                options.Password.RequireLowercase = identityOption.Password.RequireLowercase;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityOption.Lockout.DefaultLockoutTimeSpanInMins);
                options.Lockout.MaxFailedAccessAttempts = identityOption.Lockout.MaxFailedAccessAttempts;
                //options.Lockout.AllowedForNewUsers = identityOption.Lockout.AllowedForNewUsers;

                // User settings
                options.User.RequireUniqueEmail = identityOption.User.RequireUniqueEmail;
            });

            // Set token life span to 10 days
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromMinutes(identityOption.PasswordResetToken.DefaultLifespanInMin > 0
                && identityOption.PasswordResetToken.CustomLifespanEnabled ?
                identityOption.PasswordResetToken.DefaultLifespanInMin : 3624));

        }
        private void ConfigureJwtBearer(IServiceCollection services,IConfiguration configuration, FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig identityConfig)
        {
            var key = Encoding.ASCII.GetBytes(identityConfig.ApiToken.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                });
        }
    }
}
