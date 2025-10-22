using HR.Core.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FT.Core.Security
{
    [Table("Security_RefreshToken")]
    public class RefreshToken : Entity<long>
    {
        public RefreshToken(string userName, string token, string obfuscatedToken,
            string clientIpAddress, DateTime expiresAtUtc)
        {
            UserName = userName;
            Token = token;
            ObfuscatedToken = obfuscatedToken;
            ClientIpAddress = clientIpAddress;
            ExpiresAtUtc = expiresAtUtc;
        }

        public string UserName { get; set; }

        [Column("RefreshToken")]
        public string Token { get; set; }

        public string ObfuscatedToken { get; set; }

        public string ClientIpAddress { get; set; }
        public string UserSessionId { get; set; }

        public Guid? ProxyId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
