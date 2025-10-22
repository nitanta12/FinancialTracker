using FT.Core.Security.Enum;

namespace FT.Core.Security.User
{
    public interface IApplicationSignInManager
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password);

        Task SignOutAsync();
    }
}
