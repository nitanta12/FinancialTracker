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
        Task<ServiceResult<RefreshToken>> GetRefreshByClientIpToken(string username, string clientIpAddress);
        Task<ServiceResult<RefreshToken>> GetRefreshTokenByUsername(string userName);
        Task<ServiceResult<RefreshToken>> GetRefreshToken(string token);
        Task<ServiceResult<RefreshToken>> DeleteRefreshToken(string username, string sessionId);
    }
}
