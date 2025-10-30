using FT.Core.ServiceResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FT.Client.Controllers
{
    
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected OkObjectResult HrOk<T>(T resp) //where T : IServiceResult
        {
            if (resp is IServiceResult)
                return Ok(resp);
            var result = ServiceResult<T>.Success(resp);
            return Ok(result);
        }
    }
}
