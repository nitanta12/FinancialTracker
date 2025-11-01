using FT.Core.Security;
using FT.Core.Security.ClientInfo;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace FT.Client.Helper
{
    public class HttpContextAccessorProvider : IClientInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextAccessorProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string BrowserInfo
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                return httpContext?.Request?.Headers?["User-Agent"];
            }
        }

        public string BaseUri
        {
            get
            {
                var request = _httpContextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}";
            }
        }

        public string ClientIpAddress
        {
            get
            {
                if (!IsRequestAvilable())
                    return string.Empty;

                var result = string.Empty;
                try
                {
                    //Trying get Ip from forwarded header
                    if (_httpContextAccessor.HttpContext.Request.Headers is not null)
                    {
                        var forwardedHttpHeaderKey = "CloudFront-Viewer-Address";
                        var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                        if (!StringValues.IsNullOrEmpty(forwardedHeader))
                            result = forwardedHeader.FirstOrDefault();
                    }

                    if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress is not null)
                    {
                        result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    }
                }
                catch 
                {

                    return string.Empty;
                }

                if(result != null && result.Equals("::1", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = "127.0.0.1";
                }

                if (!string.IsNullOrEmpty(result))
                    result = result.Split(':').FirstOrDefault();


                return result;
            }
        }

        public string UserName
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return string.Empty;
                }

                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (claim == null)
                    return string.Empty;

                return claim.Value;
            }

        }

        public string Id
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return string.Empty;

                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (claim == null)
                    return string.Empty;

                return claim.Value;
            }
        }

        public Guid? UserID
        {
            get
            {
                var uId = Id;
                if (uId == string.Empty)
                    return Guid.Empty;

                _ = Guid.TryParse(uId, out Guid ugID);
                return ugID;
            }
        }

        public int? UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return default;

                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConsts.UserId);

                if (claim == null)
                    return default;

                return int.Parse(claim.Value);
            }
        }

        public string AuthId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return string.Empty;

                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConsts.AuthId);
                if (claim == null)
                {
                    return string.Empty;
                }

                return claim.Value;
            }
            
        }

        public bool IsCanary => throw new NotImplementedException();


        /// <summary>
        /// Checks if the current HTTP request is available
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsRequestAvilable()
        {
            if (_httpContextAccessor is null || _httpContextAccessor.HttpContext is null)
            {
                return false;
            }
            try
            {
                if (_httpContextAccessor.HttpContext.Request is null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
