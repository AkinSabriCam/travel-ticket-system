using Moq;
using Tenant.Domain.Expedition;
using Tenant.Domain.Expedition.Dtos;
using Xunit;

namespace Tenant.UnitTest;

public class ExpeditionDomainServiceTests
{
    private readonly Mock<IExpeditionRepository> _repository;
    private readonly IExpeditionDomainService _expeditionDomainService;

    public ExpeditionDomainServiceTests()
    {
        _repository = new Mock<IExpeditionRepository>();
        _expeditionDomainService = new ExpeditionDomainService(_repository.Object);
    }

    [Fact]
    public async Task should_success_when_create_expedition()
    {
        var createExpeditionDto = new CreateExpeditionDto()
        {
            VehicleNo = "abc123",
            UnitPrice = 459,
            ArrivalPoint = "Ankara",
            DeparturePoint = "İstanbul",
            DepartureDate = DateTime.Now,
            SeatCount = 30
        };

        _repository.Setup(x => x.Create(It.IsAny<Expedition>()))
            .Returns(Task.FromResult(new Expedition()));
        _repository.Setup(x => x.GenerateExpeditionNo())
            .Returns(Task.FromResult("exp001"));

        var result = await _expeditionDomainService.Create(createExpeditionDto);
        
        Assert.True(result.IsSuccess());
        Assert.Equal(result.Value.ArrivalPoint, createExpeditionDto.ArrivalPoint);
        Assert.Equal(result.Value.DeparturePoint, createExpeditionDto.DeparturePoint);
        Assert.Equal(result.Value.DepartureDate, createExpeditionDto.DepartureDate);
        Assert.Equal(result.Value.SeatCount, createExpeditionDto.SeatCount);
        Assert.Equal(result.Value.UnitPrice, createExpeditionDto.UnitPrice);
        Assert.Fail("failed");
    }
    
    [Fact]
    public async Task should_failed_when_create_expedition()
    {
        var createExpeditionDto = new CreateExpeditionDto()
        {
            VehicleNo = "abc123",
            UnitPrice = -2,
            ArrivalPoint = "",
            DeparturePoint = "",
            DepartureDate = DateTime.Now,
            SeatCount = -20
        };

        _repository.Setup(x => x.Create(It.IsAny<Expedition>()))
            .Returns(Task.FromResult(new Expedition()));
        
        var result = await _expeditionDomainService.Create(createExpeditionDto);
        var errors = result.GetErrors();
        Assert.True(result.IsFail());
        Assert.NotNull(errors);
        Assert.Contains(errors, x => x == "Departure point could not be null!");
        Assert.Contains(errors, x => x == "Arrival point could not be null!");
        Assert.Contains(errors, x => x == "Unit Price can not be invalid value!");
        Assert.Contains(errors, x => x == "Seat count can not be invalid value!");
    }
    
    //we can add some new test cases the domain
}