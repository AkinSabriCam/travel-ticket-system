using Common.User;
using Microsoft.AspNetCore.Http;

namespace Tenant.Infrastructure.User;

public class User : IUser
{
    private readonly HttpContext _httpContext;

    public User(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public Guid UserId
    {
        get
        {
            return LocalUserContext.GetUser() != null
                ? LocalUserContext.GetUser().UserId
                : IsAuthenticated()
                    ? Guid.Parse(_httpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value)
                    : Guid.Empty;
        }
        set { }
    }

    public Guid TenantId
    {
        get
        {
            return LocalUserContext.GetUser() != null
                ? LocalUserContext.GetUser().TenantId
                : IsAuthenticated()
                    ? Guid.Parse(_httpContext.User.Claims.FirstOrDefault(x => x.Type == "tenantId")?.Value)
                    : Guid.Empty;
        }
        set { }
    }
    
    public string TenantCode
    {
        get
        {
            return LocalUserContext.GetUser() != null
                ? LocalUserContext.GetUser().TenantCode
                : IsAuthenticated()
                    ? _httpContext.User.Claims.FirstOrDefault(x => x.Type == "tenantCode")?.Value
                    : string.Empty;
        }
        set { }
    }

    public bool IsMemberOfTheGroup
    {
        get =>
            LocalUserContext.GetUser() != null
                ? LocalUserContext.GetUser().IsMemberOfTheGroup
                : IsAuthenticated() && _httpContext.User.IsMemberOfTheGroup();
        set{}
    }

    public bool IsAuthenticated()
    {
        return _httpContext != null && _httpContext.User.Identity.IsAuthenticated;
    }
}