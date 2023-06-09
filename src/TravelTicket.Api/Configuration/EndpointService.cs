﻿using Master.Application.Commands.CreateTenant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.Expedition.CreateExpedition;
using Tenant.Application.Commands.Expedition.UpdateExpedition;
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

namespace TravelTicket.Api.Configuration;

public static class EndpointService
{
    public static void AddAllEndpoints(this WebApplication app)
    {
        app.MapGet("api/hello-world", () => "helloworld").RequireAuthorization();

        app.MapGet("api/expedition",
            async (IMediator mediator) => await mediator.Send(new GetAllExpeditionsQuery())).RequireAuthorization();

        app.MapGet("api/expedition/{id:guid}",
                async (IMediator mediator, Guid id) => await mediator.Send(new GetExpeditionByIdQuery(id)))
            .RequireAuthorization();

        app.MapPost("api/expedition",
                async (IMediator mediator, [FromBody] CreateExpeditionCommand command) => await mediator.Send(command))
            .RequireAuthorization();

        app.MapPost("api/expedition/search",
                async (IMediator mediator, [FromBody] SearchExpeditionsQuery query) => await mediator.Send(query))
            .RequireAuthorization();

        app.MapPut("api/expedition/{id:guid}",
                async (IMediator mediator, Guid id, [FromBody] UpdateExpeditionCommand command) =>
                    await mediator.Send(command.SetId(id)))
            .RequireAuthorization();

        app.MapGet("api/passenger",
            async (IMediator mediator) => await mediator.Send(new GetAllPassengersQuery())).RequireAuthorization();

        app.MapGet("api/passenger/{id:guid}",
                async (IMediator mediator, Guid id) => await mediator.Send(new GetPassengerByIdQuery(id)))
            .RequireAuthorization();

        app.MapPost("api/passenger",
                async (IMediator mediator, [FromBody] CreatePassengerCommand command) => await mediator.Send(command))
            .RequireAuthorization();

        app.MapPost("api/passenger/search",
                async (IMediator mediator, [FromBody] SearchPassengersQuery query) => await mediator.Send(query))
            .RequireAuthorization();

        app.MapPost("api/tenants",
            async ([FromServices] IMediator mediator, [FromBody] CreateTenantCommand command) =>
                await mediator.Send(command));

        app.MapGet("api/ticket", async (IMediator mediator) => await mediator.Send(new GetAllTicketsQuery()))
            .RequireAuthorization();

        app.MapGet("api/ticket/{id:guid}",
                async (Guid id, [FromServices] IMediator mediator) => await mediator.Send(new GetTicketByIdQuery(id)))
            .RequireAuthorization();

        app.MapPost("api/ticket",
                async (IMediator mediator, [FromBody] CreateTicketCommand command) => await mediator.Send(command))
            .RequireAuthorization();

        app.MapPost("api/ticket/{id:guid}/purchase",
                async (IMediator mediator, Guid id) => await mediator.Send(new PurchaseTicketCommand(id)))
            .RequireAuthorization();

        app.MapPost("api/ticket/{id:guid}/cancel",
                async (IMediator mediator, Guid id) => await mediator.Send(new CancelTicketCommand(id)))
            .RequireAuthorization();
    }
}