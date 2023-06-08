using MediatR;
using Tenant.Application.Queries.Ticket.GetAllTickets;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Queries.Ticket.GetTicketById;

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto>
{
    private readonly ITicketRepository _repository;

    public GetTicketByIdQueryHandler(ITicketRepository repository)
    {
        _repository = repository;
    }

    public Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetById(request.Id, TicketDto.GetProjection());
    }
}