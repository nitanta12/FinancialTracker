using HR.Core.BaseEntity;
using Microsoft.AspNetCore.Identity;


namespace FT.EntityFramework.EntityFramework.Security.Models
{
    public class ApplicationIdentityUser : IdentityUser<string>, ICreatedOn, IModifiedOn
    {
        public int? UserId { get; set; }
        public string FullName { get; set; }
        //public string ActiveDirectoryName { get; set; }
        //public string AllowableIPList { get; set; }
        //public bool DocumentIsViewOnly { get; set; }
        //public string PassCode { get; set; }
        //public bool IsTFASetupCompleted { get; set; }
        //public bool EnableTFA { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastPswResetLinkSentDate { get; set; }
        public string BackupUserId { get; set; }
        public DateTime? TwoFactorAuthenticationCodeCreatedOn { get; set; }
        //public byte TOTPState { get; set; }
        public byte Status { get; set; }
        public long? SupportUserSessionId { get; set; }
    }
}
