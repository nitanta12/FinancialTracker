using FT.Core.Security;
using FT.Services.Security.User;
using Microsoft.AspNetCore.Mvc;

namespace FT.Client.Controllers.Features.User.Controller
{
    [Route("v1/user/")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(UserCore core)
        {
            var userResp = await userService.CreateUserAsync(core, [""]).ConfigureAwait(false);
            return Ok(userResp);

        }
    }
}
