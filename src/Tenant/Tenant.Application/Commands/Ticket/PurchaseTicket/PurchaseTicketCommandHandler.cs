using MediatR;
using Tenant.Domain;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Commands.Ticket.PurchaseTicket;

public class PurchaseTicketCommandHandler : IRequestHandler<PurchaseTicketCommand>
{
    private readonly ITicketDomainService _ticketDomainService;
    private readonly ITenantUnitOfWork _unitOfWork;

    public PurchaseTicketCommandHandler(ITicketDomainService ticketDomainService, ITenantUnitOfWork unitOfWork)
    {
        _ticketDomainService = ticketDomainService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
    {
        var result = await _ticketDomainService.Purchase(request.Id);
        result.ValidateAndThrow();

        await _unitOfWork.SaveChangesAsync();
    }
}