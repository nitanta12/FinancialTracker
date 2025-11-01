using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FT.Core.Security
{
    [Table("PasswordResetTokens")]

    public class ResetTokens
    {
        [Key]
        public Guid UserRefId { get; set; }
        public string Token { get; set; }
        public byte ResetRequestSenderTypeId { get; set; }
        public DateTime ValidUpto { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }

        [NotMapped]
        public int LifespanInMin { get; set; }

    }
}
