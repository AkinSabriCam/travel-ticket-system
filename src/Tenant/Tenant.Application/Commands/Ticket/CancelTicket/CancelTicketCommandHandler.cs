using MediatR;
using Tenant.Domain;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Commands.Ticket.CancelTicket;

public class CancelTicketCommandHandler : IRequestHandler<CancelTicketCommand>
{
    private readonly ITicketDomainService _ticketDomainService;
    private readonly ITenantUnitOfWork _unitOfWork;

    public CancelTicketCommandHandler(ITenantUnitOfWork unitOfWork, ITicketDomainService ticketDomainService)
    {
        _unitOfWork = unitOfWork;
        _ticketDomainService = ticketDomainService;
    }

    public async Task Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var result = await _ticketDomainService.Cancel(request.Id);
        result.ValidateAndThrow();

        await _unitOfWork.SaveChangesAsync();
    }
}