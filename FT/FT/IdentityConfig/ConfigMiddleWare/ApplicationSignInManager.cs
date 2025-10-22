using FT.Core.Security.Enum;
using FT.Core.Security.User;
using FT.EntityFramework.EntityFramework.Security.Models;
using FT.EntityFramework.EntityFramework.Security.User.Extention;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FT.Client.IdentityConfig.ConfigMiddleWare
{
    public class ApplicationSignInManager : SignInManager<ApplicationIdentityUser>, IApplicationSignInManager
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public ApplicationSignInManager(UserManager<ApplicationIdentityUser> userManager,
            IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationIdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationIdentityUser>> logger,
            IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationIdentityUser> confirmation,
            IAuthenticationHandlerProvider handlers)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes,confirmation)
        {
            _userManager = userManager;
        }

        public async Task<SignInStatus> PasswordSignInAsync(string userName, string password)
        {
           
            var userDetail = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if(userDetail == null)
            {
                return SignInStatus.Fail;
            }


            SignInResult signInResp = await base.PasswordSignInAsync(userName, password, false, false).ConfigureAwait(false);

            return signInResp.ToApplicationIdentityUser();

        }


        public override Task SignOutAsync()
        {
            return base.SignOutAsync();
        }
    }
}
