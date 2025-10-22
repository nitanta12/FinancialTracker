using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace FT.Client.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    public class BasePrivateController : BaseController
    {

        private const string AuthSchemes =
    CookieAuthenticationDefaults.AuthenticationScheme + "," +
    JwtBearerDefaults.AuthenticationScheme;

        /// <summary>
        /// Inject Logger service
        /// </summary>
        public BasePrivateController()
        {
        }
    }
}
