using FT.Core.Security;
using FT.Core.Security.User;
using FT.EntityFramework.EntityFramework.Security.Models;
using FT.EntityFramework.EntityFramework.Security.User.Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FT.Client.IdentityConfig.ConfigMiddleWare
{
    public class ApplicationUserManagementService(IUserStore<ApplicationIdentityUser> store, IOptions<IdentityOptions> optionAccessor, IPasswordHasher<ApplicationIdentityUser> passwordHasher,
             IEnumerable<IUserValidator<ApplicationIdentityUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationIdentityUser>> passwordValidators,
             ILookupNormalizer keyNormalizer,
             IdentityErrorDescriber identityErrorDescriber,
             IServiceProvider serviceProvider,
             ILogger<UserManager<ApplicationIdentityUser>> logger
        ) : UserManager<ApplicationIdentityUser>(store, optionAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, identityErrorDescriber, serviceProvider, logger), IApplicationUserManagementService
    {
        public virtual async Task<ApplicationIdentityResult> CreateUserAsync(UserCore entity, IEnumerable<string> roles)
        {
            var applicationUserIdentity = new ApplicationIdentityUser();

            var applicationIdentity = ApplicationIdentityMiddleWare.MapApplicationIdentityUserToEntity(applicationUserIdentity, entity);

            var identityRes = string.IsNullOrEmpty(entity.Password) ?await base.CreateAsync(applicationIdentity) :
            await base.CreateAsync(applicationIdentity, entity.Password).ConfigureAwait(false);


            return identityRes == null ? null : new ApplicationIdentityResult(identityRes.Errors.Select(x => x.Description).ToList(), identityRes.Succeeded);

        }
        public async Task<DateTime> GetLockedInDateTimeAsync(string userName)
        {
            var userResp = await base.FindByNameAsync(userName).ConfigureAwait(false);

            return userResp.LockoutEnd.HasValue ? userResp.LockoutEnd.Value.LocalDateTime : DateTime.MaxValue;

        }
        public async Task<AppUserCore> FindUserByUserName(string userName)
        {
            var userResp = await base.FindByNameAsync(userName).ConfigureAwait(false);

            if(userResp == null || userResp.IsDeleted)
            {
                return null;
            }

            var user = userResp.CopyIdentityToAppuserCore();

            return user;
        }
    }
}
