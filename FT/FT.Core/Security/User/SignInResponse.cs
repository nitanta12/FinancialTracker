using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Core.Security.User
{
    public class SignInResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpireMinutes { get; set; }
        public int IdleTimeoutMinutes { get; set; }
        public int? DeviceStatusId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        //public int MfaStat { get; set; }
        public Guid? ProxyId { get; set; }
        //public Guid? TenantId { get; set; }
    }
}
