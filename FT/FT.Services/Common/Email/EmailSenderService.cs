using FT.Core.Domain.Common.Mail;
using FT.Core.ServiceResult;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FT.Services.Common.Email
{
    public class EmailSenderService(IMailSettingService mailSettingService) : IEmailSenderService
    {
        
        public async Task<ServiceResult> SendEmailAsync(string subject, string receiverAddress, 
            string body, string bcc = "", string textBody = "", string cc = "",
            List<IFormFile> files = null)
        {
            
            var serviceResult = new ServiceResult();
            try
            {
                var mailSetting = await mailSettingService.GetMailSetting().ConfigureAwait(false);

                if (mailSetting is null)
                {
                    throw new System.Exception("Email setting not found.");
                }

                using (var smtp = new SmtpClient(mailSetting.Data.HostName, mailSetting.Data.Port))
                {
                    smtp.EnableSsl = mailSetting.Data.EnableSsl;
                    smtp.Credentials = new NetworkCredential(mailSetting.Data.FromEmail, mailSetting.Data.Password);


                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(mailSetting.Data.FromEmail);
                        message.To.Add(receiverAddress);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        if (!string.IsNullOrEmpty(cc))
                            message.CC.Add(cc);

                        if(!string.IsNullOrEmpty(bcc))
                            message.Bcc.Add(bcc);

                        if (!string.IsNullOrEmpty(textBody))
                            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textBody));

                        if(files != null && files.Count > 0)
                        {
                            foreach(var file in files)
                            {
                                using var stream = file.OpenReadStream();
                                var attachment = new Attachment(stream, file.FileName);
                                message.Attachments.Add(attachment);
                            }
                        }

                        await smtp.SendMailAsync(message);
                    }
                }
                serviceResult.Message = ["Email sent successfully"];

            }
            catch (System.Exception ex)
            {

                serviceResult.Message = [$"Email failed to send : {ex.Message}"];
                serviceResult.MessageType = MessageType.Warning;
            }

            return serviceResult;
        }
    }
}
