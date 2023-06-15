using System.Net.Http.Headers;
using System.Reflection;
using Master.Application.Abstraction;
using Master.Application.Commands.CreateTenant;
using Master.Application.HttpServices;
using Master.Domain;
using Master.Domain.Tenant;
using Master.Domain.User;
using Master.Infrastructure.AppServices;
using Master.Infrastructure.EfCore;
using Master.Infrastructure.EfCore.Repositories;
using Master.Infrastructure.Keycloak;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Master.Infrastructure;

public static class StartupRegister
{
    public static void MasterRegister(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<MasterDbContext>(opt
            =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default"), 
                s=> s.MigrationsHistoryTable("__EFMigrationsHistory", "master"));
        });
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateTenantCommandHandler).Assembly));
        services.AddScoped<IMasterUnitOfWork, MasterUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantDomainService, TenantDomainService>();
        services.AddScoped<ITenantAppService, TenantAppService>();

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipeline<,>));
        
        services.AddHttpClient("keycloak", httpClient =>
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var keycloakIdentitySettings = scope.ServiceProvider.GetService<IOptions<KeycloakIdentitySettings>>();
            httpClient.BaseAddress = new Uri(keycloakIdentitySettings.Value.BaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.Configure<KeycloakIdentitySettings>(configuration.GetSection("KeycloakIdentity"));
        services.AddScoped<IIdentityHttpService, KeycloakHttpService>();

    }
}