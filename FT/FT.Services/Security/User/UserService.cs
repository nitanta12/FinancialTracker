using FT.Core.Security;
using FT.Core.Security.User;
using HR.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Services.Security.User
{
    public class UserService (IApplicationUserManagementService userManager) : IUserService
    {
        public async Task<ServiceResult<UserCore>> CreateUserAsync(UserCore entity, IEnumerable<string> roles)
        {
            IEnumerable<string> role = new List<string>();


            var userResp = await userManager.CreateUserAsync(entity, roles);

            if (userResp == null)
            {
                return new ServiceResult<UserCore>(false)
                {
                    Message = ["User cannot be created."]
                };
            }
            return new ServiceResult<UserCore>(true)
            {
                Status = userResp.Succeeded,
                Message = ["User Created successfully."]
            };

        }
    }
}
