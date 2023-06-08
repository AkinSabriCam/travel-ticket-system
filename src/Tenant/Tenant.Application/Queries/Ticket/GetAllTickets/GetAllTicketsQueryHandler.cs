using MediatR;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Queries.Ticket.GetAllTickets;

public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<TicketDto>>
{
    private readonly ITicketRepository _repository;

    public GetAllTicketsQueryHandler(ITicketRepository repository)
    {
        _repository = repository;
    }

    public Task<List<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Get(TicketDto.GetProjection());
    }
}