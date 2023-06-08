using Common.Mapping;
using MediatR;
using Tenant.Application.Queries.Ticket.GetAllTickets;
using Tenant.Domain;
using Tenant.Domain.Ticket;
using Tenant.Domain.Ticket.Dtos;

namespace Tenant.Application.Commands.Ticket.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly ITicketDomainService _ticketDomainService;
    private readonly ITenantUnitOfWork _unitOfWork;
    private readonly ICustomMapper _mapper;

    public CreateTicketCommandHandler(ITicketDomainService ticketDomainService, ITenantUnitOfWork unitOfWork,
        ICustomMapper mapper)
    {
        _ticketDomainService = ticketDomainService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var result = await _ticketDomainService.Create(_mapper.Map<CreateTicketDto>(request));
        result.ValidateAndThrow();

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TicketDto>(result.Value);
    }
}