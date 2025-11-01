using FT.Client.Controllers.Features.User.DTO.User;
using FT.Services.Security.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FT.Client.Controllers.Features.User.Controller
{
    [Route("v1/auth/")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme;

        [Route("signin")]
        [HttpPost]

        public async Task<ActionResult> SignInAsync([FromBody] SignInDto dto)
        {
            var res = await authService.SignInAsync(dto.UserName, dto.Password).ConfigureAwait(false);
            return HrOk(res);
        }


        [Route("signout")]
        [HttpPost]
        //[Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<ActionResult> SignOutAsync()
        {
            var res = await authService.SignOutAsync().ConfigureAwait(false);
            return HrOk(res);
        }


        [Route("refresh-token")]
        [HttpPost]
        public async Task<ActionResult> RefreshTokenAsync(string token)
        {
            var res = await authService.RefreshTokenAsync(refreshToken: token).ConfigureAwait(false);
            return HrOk(res);
        }


        [Route("forget-password")]
        [HttpPost]
        public async Task<ActionResult> ForgetPasswordAsync(string email)
        {
            var res = await authService.ForgetPasswordSendEmail(email).ConfigureAwait(false);
            return HrOk(res);
        }
    }
}

