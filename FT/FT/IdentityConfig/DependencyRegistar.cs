using FT.DependencyResolution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FT.Client.IdentityConfig.DependencyRegister
{
    public class DependencyRegistar
    {

        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var authenticationStartup = new AuthenticationStartup();
            authenticationStartup.ConfigureService(services, configuration);


            services.ConfigureSecurityModules();
            services.ConfigureCommonRepos();
        }
    }
}
