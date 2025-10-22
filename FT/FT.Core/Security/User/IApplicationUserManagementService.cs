using FT.Core.Security;
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

        Task<DateTime> GetLockedInDateTimeAsync(string userName);
    }
}
