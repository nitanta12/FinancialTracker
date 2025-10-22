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
    public interface IAuthService
    {
        Task<ServiceResult<SignInResponse>> SignInAsync(string userName, string password);


        Task<ServiceResult> SignOutAsync();
    }
}
