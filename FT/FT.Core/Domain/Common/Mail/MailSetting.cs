using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FT.Core.Domain.Common.Mail
{
    [Table("MailSetting")]
    public class MailSetting
    {
        [Key]
        public int Id { get; set; }
        public string FromEmail { get; set; }
        public string HostName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }

    }
}
