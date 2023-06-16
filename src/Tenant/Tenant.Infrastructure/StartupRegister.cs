using System.Reflection;
using Common.Cache;
using Common.Event;
using Common.Mapping;
using Common.User;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StackExchange.Redis;
using Tenant.Application.Commands.Passenger.CreatePassenger;
using Tenant.Domain;
using Tenant.Domain.Expedition;
using Tenant.Domain.Passenger;
using Tenant.Domain.Tenant;
using Tenant.Domain.Ticket;
using Tenant.Domain.User;
using Tenant.Infrastructure.EfCore;
using Tenant.Infrastructure.EfCore.DbContextCache;
using Tenant.Infrastructure.EfCore.Repository;
using Tenant.Infrastructure.Mapping;
using Tenant.Infrastructure.RabbitMq;
using Tenant.Infrastructure.Redis;

namespace Tenant.Infrastructure;

public static class StartupRegister
{
    public static void TenantRegister(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<TenantDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default"))
                .ReplaceService<IModelCacheKeyFactory, CustomModelCacheKeyFactory>();
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePassengerCommand).Assembly));

        services.AddScoped<ITenantUnitOfWork, TenantUnitOfWork>();

        services.AddScoped<IExpeditionRepository, ExpeditionRepository>();
        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<IUser, User.User>();
        services.AddScoped<IExpeditionDomainService, ExpeditionDomainService>();
        services.AddScoped<IPassengerDomainService, PassengerDomainService>();
        services.AddScoped<ITicketDomainService, TicketDomainService>();

        services.AddSingleton<IMapper, ServiceMapper>();

        AddMapsterMapping(services);
        AddRedisCache(services, configuration);

        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipeline<,>));

        services.AddScoped<IEventPublishService>(c =>
            {
                var settings = c.GetService<IOptions<RabbitMqSettings>>();
                var logger = c.GetRequiredService<ILogger<RabbitMqEventPublishService>>();
                var user = c.GetRequiredService<IUser>();

                var factory = new ConnectionFactory
                {
                    HostName = settings.Value.Host, 
                    Port = settings.Value.Port,
                    UserName = settings.Value.Username,
                    Password = settings.Value.Password
                };
                return new RabbitMqEventPublishService(settings, factory.CreateConnection(), user, logger);
            }
        );
    }

    private static void AddMapsterMapping(IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(mappingConfig);
        services.AddSingleton<ICustomMapper, MapsterCustomMapper>();
    }

    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisHost = configuration.GetSection("RedisSettings:Host").Value;
        var redisPort = configuration.GetSection("RedisSettings:Port").Value;
        var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { $"{redisHost}:{redisPort}" },
            AbortOnConnectFail = false,
        });
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
    }
}