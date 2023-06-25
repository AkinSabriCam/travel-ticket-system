using Master.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Tenant.Infrastructure;
using Tenant.Infrastructure.RabbitMq;
using TravelTicket.Api.EventHandlers;

namespace TravelTicket.Api.Configuration;

public static class RegisterService
{
    public static void RegisterAll(this IServiceCollection services, IConfiguration configuration)
    {
        services.TenantRegister(configuration);
        services.MasterRegister(configuration);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341", apiKey: "9idvYVCxVG73hWi3sf9V")
            .MinimumLevel.Information()
            .CreateLogger();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                var authority = configuration.GetSection("KeycloakIdentity:Authority").Value;
                var metadataAddress = configuration.GetSection("KeycloakIdentity:MetadataAddress").Value;
                var audience = configuration.GetSection("KeycloakIdentity:Audience").Value;
                
                o.MetadataAddress = metadataAddress;
                o.Authority = authority;
                o.Audience = audience;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireAudience = true,
                    ValidIssuer = audience,
                };
            });

        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));

        services.AddHostedService<ExpeditionEventHandler>();

        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();

    }
}