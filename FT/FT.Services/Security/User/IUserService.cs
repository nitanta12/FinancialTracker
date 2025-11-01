using FT.Core.Security;
using FT.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Services.Security.User
{
    public interface IUserService
    {

        Task<ServiceResult<UserCore>> CreateUserAsync(UserCore entity, IEnumerable<string> roles);

        Task<ServiceResult> ResetPassword(string userId, string tokenId, string password);
    }
}
