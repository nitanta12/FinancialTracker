using FT.Core.Infrastructure.Cryptography;
using FT.Core.Security;
using FT.Core.Security.ClientInfo;
using FT.Core.Security.User;
using FT.EntityFramework.EntityFramework.Security.Models;
using FT.Services.Security.User.RefreshTokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using FT.Core.ServiceResult;
using FT.Services.Common.Email;
namespace FT.Services.Security.User
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationSignInManager _signInManager;
        private readonly IdentityConfig _identityConfig;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IApplicationUserManagementService _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IClientInfoProvider _clientInfo;
        private readonly IEmailSenderService _emailSender;
        public readonly ClientUrlInfo _clientUrlInfo;
        //private readonly IWebHostEnvironment _hostingEnvironment;


        public AuthService(IApplicationSignInManager signInManager, IOptions<IdentityConfig> identityConfig, IHttpContextAccessor contextAccessor, 
            IApplicationUserManagementService userManager, IRefreshTokenService refreshTokenService, IClientInfoProvider clientInfo, 
            IEmailSenderService emailSender,IOptions<ClientUrlInfo> clientUrlInfo)
        {
            _signInManager = signInManager;
            _identityConfig = identityConfig.Value;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _clientInfo = clientInfo;
            _emailSender = emailSender;
            _clientUrlInfo = clientUrlInfo.Value;
        }

        public async Task<ServiceResult<SignInResponse>> SignInAsync(string userName, string password)
        {
            var user = await _userManager.FindUserByUserName(userName).ConfigureAwait(false);

            if (user == null)
            {
                return new ServiceResult<SignInResponse>(false, ["User Not found"]);
            }

            return await SignInAsync(user, password);

        }


        public async Task<ServiceResult> SignOutAsync()
        {
            var userName = _contextAccessor.HttpContext.User.Identity.IsAuthenticated ? _contextAccessor.HttpContext.User.Identity.Name : string.Empty;

            var serviceResult = new ServiceResult(true);
            var name = _clientInfo.UserName;
            if (!string.IsNullOrEmpty(userName))
                serviceResult = await _refreshTokenService.DeleteRefreshToken(userName, _clientInfo.AuthId).ConfigureAwait(false);

            await _signInManager.SignOutAsync().ConfigureAwait(false);
            await _contextAccessor.HttpContext.SignOutAsync().ConfigureAwait(false);
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return serviceResult;
        }

        public async Task<ServiceResult<SignInResponse>> RefreshTokenAsync(string refreshToken, bool validateIpAddress = true)
        {
            ServiceResult<RefreshToken> storedToken;

            if (validateIpAddress)
                storedToken = await _refreshTokenService.GetRefreshByClientIpToken(refreshToken, _clientInfo.ClientIpAddress).ConfigureAwait(false);
            else
                storedToken = await _refreshTokenService.GetRefreshToken(refreshToken).ConfigureAwait(false);

            if (storedToken == null || !storedToken.Status)
                return ServiceResult<SignInResponse>.Fail("Invalid token.");


            var expiryTime = storedToken.Data.CreatedOn.AddMinutes(_identityConfig.ApiToken.RefreshExpireMinutes);

            if (DateTime.UtcNow > expiryTime)
                return ServiceResult<SignInResponse>.Fail("Token is already expired");

            //if (!_hostingEnvironment.IsDevelopment())
            //{

            //}

            var user = await _userManager.FindUserByUserName(storedToken.Data.UserName).ConfigureAwait(false);
            var roles = new List<string> { "User" };
            var (token, expired) = GetToken(user.UserName, user.Id, roles, user.UserId.HasValue ? user.UserId.Value : 0, storedToken.Data.ProxyId, storedToken.Data.UserSessionId);

            storedToken.Data.Token = GenerateRefreshToken();
            storedToken.Data.ObfuscatedToken = token.Encrypt(AesKeys.RefreshTokenAesKey);
            storedToken.Data.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_identityConfig.ApiToken.RefreshExpireMinutes);

            await _refreshTokenService.UpdateRefreshToken(storedToken.Data).ConfigureAwait(false);

            var resp = new ServiceResult<SignInResponse>(true)
            {
                Data = new SignInResponse
                {
                    //Token based login
                    Token = token,
                    RefreshToken = storedToken.Data.Token,
                    Username = user.UserName,
                    ExpireMinutes = expired,
                    IdleTimeoutMinutes = 1,
                    ProxyId = storedToken.Data.ProxyId
                },
                Message = ["User token refreshed"]
            };

            return resp;
        }

        public async Task<ServiceResult> ForgetPasswordSendEmail(string email)
        {
            var resetUrl = _clientUrlInfo.BaseUrl + "/" + "resetpassword?token=" + "{token}" + "&verify=" + "{userId}";
            var userDetail = await _userManager.FindUserByEmailAsync(email).ConfigureAwait(false);
            if (userDetail == null)
            {
                return ServiceResult.Fail("User not found");
            }


            var (token, resetToken) = await _userManager.GenerateResetToken(userDetail.Id).ConfigureAwait(false);

            resetUrl = resetUrl.Replace("{token}", token, StringComparison.OrdinalIgnoreCase).
                       Replace("{userId}", userDetail.Id.ToString());


            var tokenValidityMessage = (resetToken == default || resetToken.LifespanInMin <= 0) ? string.Empty
                : $"The token is valid up to: {resetToken.LifespanInMin:hh\\:mm}";


            var callbackUrl = String.Format("<a href=\"{0}\">here</a>", resetUrl);

            var body = $@"
                            <html>
                            <body style='font-family: Arial, sans-serif; color: #333;'>
                                <p>Hi {userDetail.FirstName},</p>

                             <p>
                                 You (or someone pretending to be you) has initiated a request to create a new password for your RigoHR login.
                             </p>

                             <p>
                                  To create a new password, click the button below and follow the process:
                             </p>

                                <p style='text-align:'>
                                 <a href='{resetUrl}' 
                                   style='background-color: #007bff; color: white; padding: 10px 20px; 
                                         text-decoration: none; border-radius: 5px; display: inline-block;'>
                                  Reset My Password
                                 </a>
                             </p>

                             <p>
                                  If you didn't make this request, you can safely ignore this email; no changes have been made.
                                </p>

                                <p style='font-size: 0.9em; color: #777;'>
                                  This is a system-generated notification.<br>
                                  Do not reply — no one will read or respond.
                             </p>
                             </body>
                            </html>";

            var subject = "Reset your password.";

            var sendMail = await _emailSender.SendEmailAsync(subject, userDetail.Email, body).ConfigureAwait(false);

            return new ServiceResult(true) { Message = ["Email successfully sent"] };
        }
        #region Private
        private async Task<ServiceResult<SignInResponse>> SignInAsync(AppUserCore user, string password)
        {
            var signInResp = await _signInManager.PasswordSignInAsync(user.UserName, password).ConfigureAwait(false);
            var authId = Guid.NewGuid().ToString();


            _ = await SignOutAsync().ConfigureAwait(false);
            if (signInResp == Core.Security.Enum.SignInStatus.Fail)
                return new ServiceResult<SignInResponse>(false) { Message = ["Username or password incorrect."] };
            else if (signInResp == Core.Security.Enum.SignInStatus.LockedOut)
            {
                var lockedOutresp = await _userManager.GetLockedInDateTimeAsync(user.UserName).ConfigureAwait(false);
                var timeDiff = lockedOutresp.Subtract(DateTime.Now);
                return new ServiceResult<SignInResponse>(false,
                    new List<string> { $"User locked for {timeDiff.Hours}:{timeDiff.Minutes}. Try again later" });
            }
            else
            {
                await SetCookiesForAuthentication(user, authId, string.Empty, Guid.NewGuid());

                return await GenerateTokenForAuthentication(user, Guid.Empty, authId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="proxyId"></param>
        /// <param name="authId"></param>
        /// <returns></returns>
        private async Task<ServiceResult<SignInResponse>> GenerateTokenForAuthentication(AppUserCore user, Guid? proxyId, string authId)
        {
            IEnumerable<string> roles = new List<string> { "User" };

            var (token, expiryTime) = GetToken(user.UserName, user.Id, roles, user.UserId.HasValue ? user.UserId.Value : 0, proxyId, authId);

            var refreshTokenTime = DateTime.UtcNow.AddMinutes(_identityConfig.ApiToken.RefreshExpireMinutes);

            var refreshToken = new RefreshToken(user.UserName, GenerateRefreshToken(), string.Empty, "127.0.1.0", refreshTokenTime)
            {
                ObfuscatedToken = token.Encrypt(AesKeys.RefreshTokenAesKey),
                UserSessionId = authId
            };
            await _refreshTokenService.CreateRefreshToken(refreshToken).ConfigureAwait(false);
            var signIn = new SignInResponse
            {
                Email = user.Email,
                Token = token,
                RefreshToken = refreshToken.Token,
                Username = user.UserName,
                ExpireMinutes = expiryTime,
                IdleTimeoutMinutes = 100,
                DeviceStatusId = 1,
                ProxyId = proxyId,

            };
            return new ServiceResult<SignInResponse>(true)
            {
                Data = signIn,
            };

        }

        private static string GenerateRefreshToken()
        {
            var randomByte = new byte[64];

            RandomNumberGenerator.Fill(randomByte);

            return Convert.ToBase64String(randomByte);
        }
        private (string token, int expiryTime) GetToken(string userName, string id, IEnumerable<string> roles, int? userId, Guid? proxyId, string authId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_identityConfig.ApiToken.SecretKey);

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };

            if (userId != 0 && userId is not null)
                claim.Add(new Claim(AuthConsts.UserId, userId.ToString()));

            if (proxyId.HasValue)
                claim.Add(new Claim(AuthConsts.ProxyUserId, proxyId.ToString()));

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrEmpty(authId))
                claim.Add(new Claim(AuthConsts.AuthId, authId));

            var securityToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddMinutes(_identityConfig.ApiToken.AccessExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(securityToken);
            tokenHandler.WriteToken(token);

            return (tokenHandler.WriteToken(token), _identityConfig.ApiToken.AccessExpireMinutes);
        }

        private async Task SetCookiesForAuthentication(AppUserCore user, string sessionId, string expiryDate, Guid proxyId)
        {
            if (_identityConfig.Cookie.CookieBasedAuthenticationEnabled)
            {
                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(_identityConfig.Cookie.CookieExpireMinutes)
                };

                IEnumerable<string> roles = new List<string> { "User" };

                var claimIdentity = CreateIdentity(user, roles, AuthConsts.ApplicationCookie, sessionId, expiryDate, proxyId, AuthMethod.Cookie);

                string provider = "FT_App";

                await _contextAccessor.HttpContext.SignInAsync(AuthConsts.Cookies, new ClaimsPrincipal(claimIdentity));

            }
        }

        private static ClaimsIdentity CreateIdentity(AppUserCore user, IEnumerable<string> roles, string authenticationType, string authId, string expiryDate, Guid? proxyId, AuthMethod? authMethod = null)
        {
            var email = user.Email ?? "";

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email , email)
            };

            if (authMethod is not null)
                claim.Add(new Claim(AuthConsts.AuthMethod, ((byte)AuthMethod.Cookie).ToString()));

            if (!string.IsNullOrEmpty(expiryDate))
                claim.Add(new Claim(AuthConsts.ExpiryDate, expiryDate));

            if (!string.IsNullOrEmpty(user.UserName))
                claim.Add(new Claim(ClaimTypes.Name, user.UserName));

            if (!string.IsNullOrEmpty(authId))
                claim.Add(new Claim(AuthConsts.AuthId, authId));

            if (user.UserId != null)
                claim.Add(new Claim(AuthConsts.UserId, user.UserId.Value.ToString()));

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            if (proxyId.HasValue)
                claim.Add(new Claim(AuthConsts.ProxyUserId, proxyId.Value.ToString()));


            var claimsIdentity = new ClaimsIdentity(claim, authenticationType);
            return claimsIdentity;
        }

        #endregion
    }
}
