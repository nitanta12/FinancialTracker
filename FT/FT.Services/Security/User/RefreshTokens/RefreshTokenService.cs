using FT.Core.Security;
using FT.EntityFramework.EntityFramework.Security.Models;
using HR.Core.ServiceResult;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace FT.Services.Security.User.RefreshTokens
{
    public class RefreshTokenService(IRefreshTokenRepository refreshToken) : IRefreshTokenService
    {
        public async Task<ServiceResult<Core.Security.RefreshToken>> CreateRefreshToken(RefreshToken entity)
        {
            entity.CreatedOn = DateTime.UtcNow;

            await refreshToken.AddAsync(entity).ConfigureAwait(false);
            await refreshToken.CommitAsync().ConfigureAwait(false);

            return new ServiceResult<RefreshToken>(true)
            {
                Data = entity,
                Message = ["Refresh token created."]
            };
        }

        public async Task<ServiceResult<RefreshToken>> DeleteRefreshToken(string username, string sessionId)
        {
            var resp = refreshToken.Table.FirstOrDefault(x => x.UserName == username && x.UserSessionId == sessionId);
            if(resp != null)
            {
                refreshToken.Delete(resp);
                await refreshToken.CommitAsync().ConfigureAwait(false);
            }
            return new ServiceResult<RefreshToken>(true);
        }

        public async Task<RefreshToken> GetRefreshToken(string username, string sessionId)
        {
            var entity = await refreshToken.GetAsync(x => x.UserName == username && x.UserSessionId == sessionId).ConfigureAwait(false);
            return entity;
        }
        public async Task<ServiceResult<RefreshToken>> GetRefreshByClientIpToken(string username, string clientIpAddress)
        {
            var entity = await refreshToken.GetAsync(x => x.UserName == username && x.ClientIpAddress == clientIpAddress).ConfigureAwait(false);
            if (entity == null)
                return new ServiceResult<RefreshToken>(false);
            return new ServiceResult<RefreshToken>(true) { Data = entity };
        }

        public async Task<ServiceResult<RefreshToken>> GetRefreshTokenByUsername(string username)
        {
            var entity = await refreshToken.GetAsync(x => x.UserName == username ).ConfigureAwait(false);
            if (entity == null)
                return new ServiceResult<RefreshToken>(false);
            return new ServiceResult<RefreshToken>(true) { Data = entity };
        }
        public async Task<ServiceResult<RefreshToken>> GetRefreshToken(string token)
        {
            var entity = await refreshToken.GetAsync(x => x.Token == token).ConfigureAwait(false);
            if (entity == null)
                return new ServiceResult<RefreshToken>(false);
            return new ServiceResult<RefreshToken>(true) { Data = entity };
        }
        public async Task<ServiceResult<RefreshToken>> UpdateRefreshToken(RefreshToken entity)
        {
            entity.CreatedOn = DateTime.UtcNow;

             refreshToken.Update(entity);
            await refreshToken.CommitAsync().ConfigureAwait(false);

            return new ServiceResult<RefreshToken>(true)
            {
                Data = entity,
                Message = ["Refresh token updated."]
            };
        }
    }
}
