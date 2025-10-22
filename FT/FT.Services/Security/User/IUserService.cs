﻿using FT.Core.Security;
using HR.Core.ServiceResult;
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
    }
}
