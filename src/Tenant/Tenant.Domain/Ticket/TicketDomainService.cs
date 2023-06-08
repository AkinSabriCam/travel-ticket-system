using Common.Validation;
using Tenant.Domain.Expedition;
using Tenant.Domain.Ticket.Dtos;

namespace Tenant.Domain.Ticket;

public class TicketDomainService : ITicketDomainService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IExpeditionRepository _expeditionRepository;

    public TicketDomainService(ITicketRepository ticketRepository, IExpeditionRepository expeditionRepository)
    {
        _ticketRepository = ticketRepository;
        _expeditionRepository = expeditionRepository;
    }

    public async Task<Result<Ticket>> Create(CreateTicketDto createDto)
    {
        var expedition = await _expeditionRepository.GetById(createDto.ExpeditionId, includes: x => x.Tickets);

        if (!expedition.IsAvailableForSale())
            return Result<Ticket>.Fail("This Expedition is not available for sale!");

        var entity = new Ticket
        {
            ExpeditionId = createDto.ExpeditionId,
            PassengerId = createDto.PassengerId,
            Status = createDto.Status,
            DepartureDate = expedition.DepartureDate,
            Price = expedition.UnitPrice
        };

        await _ticketRepository.Create(entity);
        return Result<Ticket>.Ok(entity);
    }

    public async Task<Result> Purchase(Guid id)
    {
        var ticket = await _ticketRepository.GetById(id);
        var expedition =
            await _expeditionRepository.GetById(ticket.ExpeditionId.GetValueOrDefault(), includes: x => x.Tickets);

        var result = Result.Ok();
        if (ticket.Status != TicketStatus.Reserved)
            result.AddError("Ticket status should be reserved");

        if (!expedition.IsAvailableForSale())
            result.AddError("This Expedition is not available for sale!");

        if (result.IsFail())
            return result;

        ticket.AddHistory();
        ticket.Status = TicketStatus.Purchased;
        await _ticketRepository.Update(ticket, ticket.Id);

        return Result.Ok();
    }

    public async Task<Result> Cancel(Guid id)
    {
        var ticket = await _ticketRepository.GetById(id);

        var result = Result.Ok();
        if (ticket.Status == TicketStatus.Cancelled)
            result.AddError("Ticket status should be reserved or purchased");

        if (result.IsFail())
            return result;

        ticket.AddHistory();
        ticket.Status = TicketStatus.Cancelled;
        await _ticketRepository.Update(ticket, ticket.Id);

        return Result.Ok();
    }
}