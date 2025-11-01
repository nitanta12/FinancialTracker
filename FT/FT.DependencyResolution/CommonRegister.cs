using FT.Core.Domain.Common.Mail.Infrastructure;
using FT.EntityFramework.Repository.Common.Mail;
using FT.Services.Common.Email;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.DependencyResolution
{
    public static  class CommonRegister
    {
        public static void ConfigureCommonRepos(this IServiceCollection services)
        {
            services.AddScoped<IMailSettingRepository, MailSettingRepository>();
            services.AddScoped<IMailSettingService, MailSettingService>();

            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
