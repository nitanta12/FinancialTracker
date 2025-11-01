using FT.Core.Security;
using FT.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Core.Security.User
{
    public interface IApplicationUserManagementService : IDisposable
    {

        Task<ApplicationIdentityResult> CreateUserAsync(UserCore entity, IEnumerable<string> roles);

        Task<AppUserCore> FindUserByUserName(string userName);
        Task<AppUserCore> FindUserByUserId(string userId);

        Task<DateTime> GetLockedInDateTimeAsync(string userName);

        Task<AppUserCore> FindUserByEmailAsync(string email);

        Task<ServiceResult<ResetTokens>> SaveToken(string userId, string token);
        Task<(string code, ResetTokens resetToken)> GenerateResetToken(string userId);

        Task<ApplicationIdentityResult> ResetPasswordAsync(string userId, string password, string tokenId);
    }
}
