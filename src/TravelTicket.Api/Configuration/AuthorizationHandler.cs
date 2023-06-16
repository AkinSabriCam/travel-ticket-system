using Microsoft.AspNetCore.Authorization;
using Tenant.Infrastructure.User;

namespace TravelTicket.Api.Configuration;

public class AuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if(context.User.Identity is not { IsAuthenticated: true })
            context.Fail();
        
        if(!context.User.IsMemberOfTheGroup())
            context.Fail();

        return Task.CompletedTask;
    }
}