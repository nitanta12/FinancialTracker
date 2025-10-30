using FT.Core.Domain.Common.Mail;
using FT.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Services.Common.Email
{
    public interface IMailSettingService
    {
        Task<ServiceResult<MailSetting>> GetMailSetting();

    }
}
