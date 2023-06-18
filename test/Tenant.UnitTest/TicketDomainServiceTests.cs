using System.Linq.Expressions;
using Moq;
using Tenant.Domain.Expedition;
using Tenant.Domain.Ticket;
using Tenant.Domain.Ticket.Dtos;
using Xunit;

namespace Tenant.UnitTest;

public class TicketDomainServiceTests
{
    private readonly Mock<ITicketRepository> _repository;
    private readonly Mock<IExpeditionRepository> _expeditionRepository;
    private readonly ITicketDomainService _ticketDomainService;

    public TicketDomainServiceTests()
    {
        _repository = new Mock<ITicketRepository>();
        _expeditionRepository = new Mock<IExpeditionRepository>();
        _ticketDomainService = new TicketDomainService(_repository.Object, _expeditionRepository.Object);
    }

    [Fact]
    public async Task should_success_when_create_ticket()
    {
        var createTicketDto = new CreateTicketDto()
        {
            ExpeditionId = Guid.NewGuid(),
            PassengerId = Guid.NewGuid(),
            Status = TicketStatus.Reserved,
            SeatNumber = "4"
        };
        var expedition = new Expedition();
        expedition.SetSeatCount(20);

        _repository.Setup(x => x.Create(It.IsAny<Ticket>()))
            .Returns(Task.FromResult(new Ticket()));
        _expeditionRepository.Setup(
                x => x.GetById(It.IsAny<Guid>(), It.IsAny<Expression<Func<Expedition, object>>[]>()))
            .Returns(Task.FromResult(expedition));

        var result = await _ticketDomainService.Create(createTicketDto);

        Assert.True(result.IsSuccess());
        Assert.Equal(result.Value.ExpeditionId, createTicketDto.ExpeditionId);
        Assert.Equal(result.Value.PassengerId, createTicketDto.PassengerId);
        Assert.Equal(result.Value.Status, createTicketDto.Status);
        Assert.Equal(result.Value.SeatNumber, createTicketDto.SeatNumber);
    }

    [Fact]
    public async Task should_failed_when_create_ticket()
    {
        var createTicketDto = new CreateTicketDto()
        {
            ExpeditionId = Guid.NewGuid(),
            PassengerId = Guid.NewGuid(),
            Status = TicketStatus.Reserved,
            SeatNumber = string.Empty
        };
        var expedition = new Expedition();
        expedition.SetSeatCount(20);

        _repository.Setup(x => x.Create(It.IsAny<Ticket>()))
            .Returns(Task.FromResult(new Ticket()));
        _expeditionRepository.Setup(
                x => x.GetById(It.IsAny<Guid>(), It.IsAny<Expression<Func<Expedition, object>>[]>()))
            .Returns(Task.FromResult(expedition));
        
        var result = await _ticketDomainService.Create(createTicketDto);
        var errors = result.GetErrors();

        Assert.True(result.IsFail());
        Assert.NotNull(errors);
        Assert.Contains(errors, x => x == "Seat no can not be empty!");
    }

    [Fact]
    public async Task should_failed_when_purchase_ticket()
    {
        var ticket = new Ticket()
        {
            Status = TicketStatus.Cancelled
        };
        var expedition = new Expedition();
        expedition.SetSeatCount(20);

        _repository.Setup(x => x.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(ticket));
        _expeditionRepository.Setup(
                x => x.GetById(It.IsAny<Guid>(), It.IsAny<Expression<Func<Expedition, object>>[]>()))
            .Returns(Task.FromResult(expedition));
        
        var result = await _ticketDomainService.Purchase(ticket.Id);
        var errors = result.GetErrors();

        Assert.True(result.IsFail());
        Assert.NotNull(errors);
        Assert.Contains(errors, x => x == "Ticket status should be reserved!");
    }
    
    [Fact]
    public async Task should_failed_when_cancel_ticket()
    {
        var ticket = new Ticket()
        {
            Status = TicketStatus.Cancelled
        };

        _repository.Setup(x => x.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(ticket));
        
        var result = await _ticketDomainService.Cancel(ticket.Id);
        var errors = result.GetErrors();

        Assert.True(result.IsFail());
        Assert.NotNull(errors);
        Assert.Contains(errors, x => x == "Ticket status should be reserved or purchased!");
    }
    
    //we can add some new test cases the domain
}