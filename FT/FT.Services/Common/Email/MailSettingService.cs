using FT.Core.Domain.Common.Mail;
using FT.Core.Domain.Common.Mail.Infrastructure;
using FT.Core.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Services.Common.Email
{
    public class MailSettingService(IMailSettingRepository mailRepo) : IMailSettingService
    {
        public async Task<ServiceResult<MailSetting>> GetMailSetting()
        {
            var res = await mailRepo.Table.ToListAsync().ConfigureAwait(false);

            return new ServiceResult<MailSetting>(true)
            {
                Data = res.FirstOrDefault()
            };
        }
    }
}
