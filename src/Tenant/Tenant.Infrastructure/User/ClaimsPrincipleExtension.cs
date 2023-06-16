using System.Security.Claims;

namespace Tenant.Infrastructure.User;

public static class ClaimsPrincipleExtension
{
    public static bool IsMemberOfTheGroup(this ClaimsPrincipal user)
    {
        return user.Claims.Any(x => x.Type == "memberOf" && x.Value == "BackOffice");
    }
}