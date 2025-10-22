namespace FT.EntityFramework.EntityFramework.Security.Models
{
    public class IdentityConfig
    {
        public PasswordElement Password { get; set; }

        public UserElement User { get; set; }

        public LockoutElement Lockout { get; set; }

       // public CustomElement Custom { get; set; }
        public CookieElement Cookie { get; set; }

        public ApiToken ApiToken { get; set; }
        public PasswordResetToken PasswordResetToken { get; set; }
        //public TFA TFA { get; set; }
        public Session Session { get; set; }
    }
    //public class TFA
    //{
    //    public int TfaRecoveryCodeExpireMinutes { get; set; }
    //}
    public class Session
    {
        public int IdleTimeoutMinutes { get; set; }
    }
    public class PasswordElement
    {
        public int RequiredLength { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
    }




    public class UserElement
    {
        public bool RequireUniqueEmail { get; set; }
    }


    //public class TokenElement : ITokenElement
    //{
    //    //[ConfigurationProperty("TokenEndpointPath")]
    //    //public string TokenEndpointPath => (String)this["TokenEndpointPath"];

    //    //[ConfigurationProperty("AccessTokenExpireTimeSpan")]
    //    //public int AccessTokenExpireTimeSpan => (int)this["AccessTokenExpireTimeSpan"];
    //    public string TokenEndpointPath { get; set; }

    //    public int AccessTokenExpireTimeSpan { get; set; }
    //}
    public class LockoutElement
    {
        public bool AllowedForNewUsers { get; set; }
        public int DefaultLockoutTimeSpanInMins { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }

    //public class CustomElement
    //{
    //    public bool PreventMultipleLoginForSameUser { get; set; }
    //    public int UserIdleTimeoutMinutes { get; set; }
    //}
    public class CookieElement
    {
        public bool CookieBasedAuthenticationEnabled { get; set; }
        public string SharedCookieDomain { get; set; }
        public int CookieExpireMinutes { get; set; }
        public string SharedCookieSecret { get; set; }
    }
    public class ApiToken
    {
        public string SecretKey { get; set; }
        public int AccessExpireMinutes { get; set; }
        public int RefreshExpireMinutes { get; set; }
    }
    public class PasswordResetToken
    {
        public bool CustomLifespanEnabled { get; set; }
        public int DefaultLifespanInMin { get; set; }
        public int SelfRequestedLifespanInMin { get; set; }
        public int AdminRequestedLifespanInMin { get; set; }
    }
}
