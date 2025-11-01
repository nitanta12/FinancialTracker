using FT.Core.ServiceResult;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FT.Services.Common.Email
{
    public interface IEmailSenderService
    {

        Task<ServiceResult> SendEmailAsync(string subject, string receiverAddress, string body, string bcc = "",string textBody = "", string cc = "", List<IFormFile> files = null); 
    }
}
