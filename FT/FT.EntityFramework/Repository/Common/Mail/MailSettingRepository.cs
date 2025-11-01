using FT.Core.Domain.Common.Mail;
using FT.Core.Domain.Common.Mail.Infrastructure;
using FT.EntityFramework.EntityFramework.DbContext;
using FT.EntityFramework.EntityFramework.Repository;

namespace FT.EntityFramework.Repository.Common.Mail
{
    public class MailSettingRepository(ApplicationDbContext dbContext): Repository<MailSetting>(dbContext),IMailSettingRepository
    {
    }
}
