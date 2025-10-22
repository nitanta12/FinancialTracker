namespace FT.Core.Security
{
    public static class AuthConsts
    {
        public static string CookieBasedAuthenticationEnabled = "CookieBasedAuthenticationEnabled";
        public static string ApplicationCookie = "ApplicationCookie";
        public static string Cookies = "Cookies";
        public static string RoleTypeId = "RoleTypeId";
        public static string ProxyUserId = "ProxyUserId";
        public static string ProxyLoginDirectory = "ProxyCascade";
        public static string UserId = "EmployeeId";
        public static string TenantId = "TenantId";

        /// <summary>
        /// Same key used in the workhorse 
        /// </summary>
        public static string AuthId = "AuthId";
        public const string P2PIdentityPolicy = "P2P_Identity";
        public const string SubscriptionId = nameof(SubscriptionId);
        public const string AppCheckHeaderId = "X-Firebase-AppCheck";
        public const string ExternalUserPermissionKey = "Permissions";
        public const string ApplicationId = "ApplicaitonId";
        //public const string GroupUserAdmin = "GroupUserAdmin";
        public const string Roles = "Roles";
        public const string ExpiryDate = "ExpiryDate";
        //public const string UserType = nameof(UserType);
        //public const string SupportUserSessionId = "support.usr.session.id";
        public const string CanaryCookieName = "canary";
        public const string CanaryCookieAlwaysValue = "always";
        public const string CanaryCookieNeverValue = "never";
        public static string IsCanary = "IsCanary";
        public const string CsSessionCookieName = "cs.session.cookie";
        public const string AuthMethod = "auth_method";
        public const string CsSessionCookieEncryptionKey = "{0}_cs_request_verification_key";//cs request id 
    }
    public enum AuthMethod
    {
        Cookie = 1, Jwt = 2
    }
}
