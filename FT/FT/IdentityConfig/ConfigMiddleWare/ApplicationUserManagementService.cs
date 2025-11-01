using FT.Core.Security;
using FT.Core.Security.Infrastructure;
using FT.Core.Security.User;
using FT.Core.ServiceResult;
using FT.EntityFramework.EntityFramework.DbContext;
using FT.EntityFramework.EntityFramework.Security.Models;
using FT.EntityFramework.EntityFramework.Security.User.Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FT.Client.IdentityConfig.ConfigMiddleWare
{
    public class ApplicationUserManagementService
      : UserManager<ApplicationIdentityUser>, IApplicationUserManagementService
    {
        private readonly FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig _identityConfig;
        protected ApplicationDbContext _dbContext;
        private readonly IResetTokenRepository _resetTokenRepository;
        public ApplicationUserManagementService(
            IUserStore<ApplicationIdentityUser> store,
            IOptions<IdentityOptions> optionAccessor,
            IPasswordHasher<ApplicationIdentityUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationIdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationIdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber identityErrorDescriber,
            IServiceProvider serviceProvider,
            ILogger<UserManager<ApplicationIdentityUser>> logger,
            IOptions<FT.EntityFramework.EntityFramework.Security.Models.IdentityConfig> identityConfig
,
            ApplicationDbContext dbContext,
            IResetTokenRepository resetTokenRepository)
            : base(store, optionAccessor, passwordHasher, userValidators, passwordValidators,
                  keyNormalizer, identityErrorDescriber, serviceProvider, logger)
        {

            _identityConfig = identityConfig.Value;
            _dbContext = dbContext;
            _resetTokenRepository = resetTokenRepository;
        }
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
        public async Task<AppUserCore> FindUserByUserId(string userId)
        {
            var userResp = await base.FindByIdAsync(userId).ConfigureAwait(false);
            if(userResp == null || userResp.IsDeleted)
            {
                return null;
            }

            var user = userResp.CopyIdentityToAppuserCore();

            return user;
        }

        public async Task<AppUserCore> FindUserByEmailAsync(string email)
        {
            var userResp = await base.FindByEmailAsync(email).ConfigureAwait(false);

            if(userResp == null || userResp.IsDeleted)
            {
                return null;
            }

            var user = userResp.CopyIdentityToAppuserCore();

            return user;
        }

        public async Task<ServiceResult<ResetTokens>> SaveToken(string userId, string token)
        {
            if (!_identityConfig.PasswordResetToken.CustomLifespanEnabled)
                return ServiceResult<ResetTokens>.Success(string.Empty);
            CancellationToken cancellationToken = CancellationToken.None;
            Guid userGuid = Guid.Parse(userId);

            var dbToken = _dbContext.ResetTokens.FirstOrDefault(x => x.UserRefId == userGuid);
            var lifeSpan = _identityConfig.PasswordResetToken.DefaultLifespanInMin;
            if (dbToken is not null)
            {
                dbToken.UserRefId = userGuid;
                dbToken.ResetRequestSenderTypeId = 1;
                dbToken.ValidUpto = DateTime.Now.AddMinutes(lifeSpan);
                dbToken.Token = token;

                _dbContext.ResetTokens.Update(dbToken);
                await _dbContext.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                 dbToken = new ResetTokens
                {
                    UserRefId = userGuid,
                    ResetRequestSenderTypeId = 1,
                    ValidUpto = DateTime.Now.AddMinutes(lifeSpan),
                    Token = token,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userGuid
                };
                await _dbContext.ResetTokens.AddAsync(dbToken).ConfigureAwait(false);
                await _dbContext.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            dbToken.LifespanInMin = lifeSpan;

            return ServiceResult<ResetTokens>.Success(dbToken);
        }
        public async Task<(string code, ResetTokens resetToken)> GenerateResetToken(string userId)
        {
            var user =  base.Users.FirstOrDefault(e => e.Id == userId);
            var token = await base.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

            ResetTokens resetToken = default;
            if(!string.IsNullOrEmpty(token))
            {
                var saveToken = await SaveToken(userId,token).ConfigureAwait(false);

                if (saveToken.Status)
                    resetToken = saveToken.Data;
            }

            return (token, resetToken);
        }

        ValueTask<bool> VerifyResetToken(string userId)
        {
            if (!_identityConfig.PasswordResetToken.CustomLifespanEnabled)
                return new ValueTask<bool>(true);

            Guid userGuid = Guid.Parse(userId);

            var dbToken = _dbContext.ResetTokens.FirstOrDefault(x => x.UserRefId == userGuid);

            if(dbToken is null)
                return new ValueTask<bool>(false);

            if(dbToken.ValidUpto >DateTime.Now)
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(false);
        }

        public async Task<ApplicationIdentityResult> ResetPasswordAsync(string userId, string password, string tokenId)
        {
            var user = await base.FindByIdAsync(userId).ConfigureAwait(false);

            if (!await VerifyResetToken(userId))
                return new ApplicationIdentityResult(["Token expired."], false);

            var identityResult = await base.ResetPasswordAsync(user, tokenId, password).ConfigureAwait(false);


            return identityResult == null ? null : new ApplicationIdentityResult(identityResult.Errors.Select(x => x.Description).ToList(), identityResult.Succeeded);
        }
    }
}
