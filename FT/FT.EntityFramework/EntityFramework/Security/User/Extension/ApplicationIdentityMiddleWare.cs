using FT.Core.Security;
using FT.Core.Security.Enum;
using FT.EntityFramework.EntityFramework.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Runtime.CompilerServices;

namespace FT.EntityFramework.EntityFramework.Security.User.Extention
{
    public static class ApplicationIdentityMiddleWare
    {

        public static SignInStatus ToApplicationIdentityUser( this SignInResult identityResult)
        {
            if (identityResult.Succeeded)
                return SignInStatus.Success;
            else if (identityResult.IsLockedOut)
                return SignInStatus.LockedOut;
            else
                return SignInStatus.Fail;
        }
        public static ApplicationIdentityUser MapApplicationIdentityUserToEntity(ApplicationIdentityUser entity, UserCore dto)
        {
            if(dto == null)
            {
                return null;
            }
            var Id = Guid.NewGuid().ToString();
            entity.UserName = dto.UserName;
            entity.FullName = dto.FirstName + " " + dto.LastName;
            entity.Email = dto.Email;
            entity.Id = Id;
            entity.UserId = dto.UserId;
            entity.LockoutEnabled = false;
            entity.TwoFactorEnabled = false;
            entity.IsDeleted = false;
            entity.Status = 1;
            entity.CreatedBy = Id;
            entity.CreatedOn = DateTime.Now;

            return entity;
        }


        public static AppUserCore CopyIdentityToAppuserCore(this ApplicationIdentityUser applicationIdentityUser)
        {
            AppUserCore entity = new AppUserCore();
            if(entity == null)
            {
                return null;
            }
            if(applicationIdentityUser == null)
            {
                return null;
            }

            entity.UserName = applicationIdentityUser.UserName;
            entity.Id = applicationIdentityUser.Id;
            entity.Email = applicationIdentityUser.Email;
            entity.UserId = applicationIdentityUser.UserId;
            var name = applicationIdentityUser.FullName.Split(" ");
            entity.FirstName = name[0];
            entity.LastName = name[1];

            return entity;
        }
    }
}
