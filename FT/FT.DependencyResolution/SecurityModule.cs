using FT.Core.Security;
using FT.EntityFramework.Repository;
using FT.Services.Security.User;
using FT.Services.Security.User.RefreshTokens;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.DependencyResolution
{
    public static class SecurityModule
    {
        public static void ConfigureSecurityModules(this IServiceCollection services)
        {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();


            //refreshToken
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        }
    }
}
