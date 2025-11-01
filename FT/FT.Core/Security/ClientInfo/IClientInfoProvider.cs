namespace FT.Core.Security.ClientInfo
{
    public interface IClientInfoProvider
    {
        string BrowserInfo { get;  }
        string ClientIpAddress { get;  }
        string UserName { get;  }
        string Id { get;  }
        Guid? UserID { get;  }
        string AuthId { get;  }

        int? UserId { get;  }

        bool IsCanary { get; }
        string BaseUri { get; }

    }
}
