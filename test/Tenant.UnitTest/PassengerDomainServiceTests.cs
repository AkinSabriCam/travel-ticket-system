using Moq;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;
using Xunit;

namespace Tenant.UnitTest;

public class PassengerDomainServiceTests
{
    private readonly Mock<IPassengerRepository> _repository;
    private readonly IPassengerDomainService _passengerDomainService;

    public PassengerDomainServiceTests()
    {
        _repository = new Mock<IPassengerRepository>();
        _passengerDomainService = new PassengerDomainService(_repository.Object);
    }

    [Fact]
    public async Task should_success_when_create_passenger()
    {
        var createPassengerDto = new CreatePassengerDto()
        {
            Identity = "55555555555",
            FirstName = "Jane",
            LastName = "Doe",
        };

        _repository.Setup(x => x.Create(It.IsAny<Passenger>()))
            .Returns(Task.FromResult(new Passenger()));
        
        var result = await _passengerDomainService.Create(createPassengerDto);
        
        Assert.True(result.IsSuccess());
        Assert.Equal(result.Value.FirstName, createPassengerDto.FirstName);
        Assert.Equal(result.Value.LastName, createPassengerDto.LastName);
        Assert.Equal(result.Value.Identity, createPassengerDto.Identity);
    }
    
    [Fact]
    public async Task should_failed_when_create_passenger()
    {
        var createPassengerDto = new CreatePassengerDto()
        {
            Identity = "55555555555a",
            FirstName = string.Empty,
            LastName = string.Empty,
        };

        _repository.Setup(x => x.Create(It.IsAny<Passenger>()))
            .Returns(Task.FromResult(new Passenger()));
        
        var result = await _passengerDomainService.Create(createPassengerDto);
        var errors = result.GetErrors();
        
        Assert.True(result.IsFail());
        Assert.NotNull(errors);
        Assert.Contains(errors, x => x == "Firstname can not be empty!");
        Assert.Contains(errors, x => x == "Lastname can not be empty!");
        Assert.Contains(errors, x => x == "Identity length is not 11!");
    }
    
    //we can add some new test cases the domain
}