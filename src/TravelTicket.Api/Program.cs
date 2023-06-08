using Master.Application.Commands.CreateTenant;
using Master.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StackExchange.Redis;
using Tenant.Application.Commands.Expedition.CreateExpedition;
using Tenant.Application.Commands.Passenger.CreatePassenger;
using Tenant.Application.Commands.Ticket.CancelTicket;
using Tenant.Application.Commands.Ticket.CreateTicket;
using Tenant.Application.Commands.Ticket.PurchaseTicket;
using Tenant.Application.Queries.Expedition.GetAllExpeditions;
using Tenant.Application.Queries.Expedition.GetExpeditionById;
using Tenant.Application.Queries.Expedition.SearchExpeditions;
using Tenant.Application.Queries.Passenger.GetAllPassengers;
using Tenant.Application.Queries.Passenger.GetPassengerById;
using Tenant.Application.Queries.Passenger.SearchPassengers;
using Tenant.Application.Queries.Ticket.GetAllTickets;
using Tenant.Application.Queries.Ticket.GetTicketById;
using Tenant.Infrastructure;
using Tenant.Infrastructure.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.TenantRegister(builder.Configuration);
builder.Services.MasterRegister(builder.Configuration);

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341", apiKey: "9idvYVCxVG73hWi3sf9V")
    .MinimumLevel.Information()
    .CreateLogger();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.MetadataAddress = "http://localhost:6743/realms/travel_ticket/.well-known/openid-configuration";
        o.Authority = "http://localhost:6743/realms/";
        o.Audience = "travel_ticket_system_resource";
        o.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));

var redisHost = builder.Configuration.GetValue<string>("RedisSettings:Host");
var redisPort = builder.Configuration.GetValue<int>("RedisSettings:Port");
var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { $"{redisHost}:{redisPort}" },
    AbortOnConnectFail = false,
});


builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

var app = builder.Build();

#region endpoints

app.MapGet("api/expedition",
    async (IMediator mediator) => await mediator.Send(new GetAllExpeditionsQuery())).RequireAuthorization();

app.MapGet("api/expedition/{id:guid}",
    async (IMediator mediator, Guid id) => await mediator.Send(new GetExpeditionByIdQuery(id))).RequireAuthorization();

app.MapPost("api/expedition",
        async (IMediator mediator, [FromBody] CreateExpeditionCommand command) => await mediator.Send(command))
    .RequireAuthorization();

app.MapPost("api/expedition/search",
        async (IMediator mediator, [FromBody] SearchExpeditionsQuery query) => await mediator.Send(query))
    .RequireAuthorization();

app.MapGet("api/passenger",
    async (IMediator mediator) => await mediator.Send(new GetAllPassengersQuery())).RequireAuthorization();

app.MapGet("api/passenger/{id:guid}",
    async (IMediator mediator, Guid id) => await mediator.Send(new GetPassengerByIdQuery(id))).RequireAuthorization();

app.MapPost("api/passenger",
        async (IMediator mediator, [FromBody] CreatePassengerCommand command) => await mediator.Send(command))
    .RequireAuthorization();

app.MapPost("api/passenger/search",
        async (IMediator mediator, [FromBody] SearchPassengersQuery query) => await mediator.Send(query))
    .RequireAuthorization();

app.MapPost("api/tenants",
    async ([FromServices] IMediator mediator, [FromBody] CreateTenantCommand command) =>
        await mediator.Send(command));

app.MapGet("api/ticket", async (IMediator mediator) => { await mediator.Send(new GetAllTicketsQuery()); });

app.MapGet("api/ticket/{id:guid}",
    async (Guid id, [FromServices] IMediator mediator) => { await mediator.Send(new GetTicketByIdQuery(id)); });

app.MapPost("api/ticket",
        async (IMediator mediator, [FromBody] CreateTicketCommand command) => await mediator.Send(command))
    .RequireAuthorization();

app.MapPost("api/ticket/{id:guid}/purchase",
        async (IMediator mediator, Guid id) => await mediator.Send(new PurchaseTicketCommand(id)))
    .RequireAuthorization();

app.MapPost("api/ticket/{id:guid}/cancel",
        async (IMediator mediator, Guid id) => await mediator.Send(new CancelTicketCommand(id)))
    .RequireAuthorization();

#endregion

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.Run();