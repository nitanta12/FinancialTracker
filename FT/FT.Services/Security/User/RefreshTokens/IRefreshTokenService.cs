using HR.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FT.Core.Security;

namespace FT.Services.Security.User.RefreshTokens
{
    public interface IRefreshTokenService
    {
        Task<ServiceResult<RefreshToken>> CreateRefreshToken(RefreshToken entity);
        Task<ServiceResult<RefreshToken>> UpdateRefreshToken(RefreshToken entity);
        Task<RefreshToken> GetRefreshToken(string username, string sessionId);
        Task<RefreshToken> GetRefreshByClientIpToken(string username, string clientIpAddress);
        Task<RefreshToken> GetRefreshByClientIpToken(string username);
        Task<ServiceResult<RefreshToken>> DeleteRefreshToken(string username, string sessionId);
    }
}
