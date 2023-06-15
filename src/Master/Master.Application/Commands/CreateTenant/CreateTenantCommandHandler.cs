using Common;
using Common.User;
using Master.Application.Abstraction;
using Master.Application.Abstraction.Dto;
using Master.Application.HttpServices;
using Master.Application.HttpServices.RequestModels;
using Master.Application.Queries.GetAllTenants;
using Master.Domain;
using Master.Domain.Tenant;
using Master.Domain.Tenant.Dto;
using Master.Domain.User;
using MediatR;

namespace Master.Application.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly ITenantDomainService _tenantDomainService;
    private readonly IUserRepository _userRepository;
    private readonly IIdentityHttpService _identityHttpService;
    private readonly ITenantAppService _tenantAppService;
    private readonly IMasterUnitOfWork _masterUnitOfWork;

    public CreateTenantCommandHandler(ITenantDomainService tenantDomainService, IMasterUnitOfWork masterUnitOfWork,
        IIdentityHttpService identityHttpService, ITenantAppService tenantAppService, IUserRepository userRepository)
    {
        _tenantDomainService = tenantDomainService;
        _masterUnitOfWork = masterUnitOfWork;
        _identityHttpService = identityHttpService;
        _tenantAppService = tenantAppService;
        _userRepository = userRepository;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        var createTenantDto = GetCreateTenantDto(request);
        var result = await _tenantDomainService.Create(createTenantDto, userId);

        result.ValidateAndThrow();
        
        await LocalUserContext.SetUser(new LocalUser()
        {
            TenantCode = result.Value.Code,
            TenantId = result.Value.Id,
            UserId = userId
        });
        await CreateUser(request, userId);

        await _masterUnitOfWork.SaveChangesAsync();
        await _identityHttpService.CreateUser(new CreateUserRequest()
        {
            Username = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Attributes = new Dictionary<string, List<string>>
            {
                { "tenantCode", new List<string>() { request.Code } },
                { "tenantId", new List<string>() { result.Value.Id.ToString() } },
                { "userId", new List<string>() { userId.ToString() } },
                { "userType", new List<string>() { UserType.TenantOwner.ToString() } },
            }
        }, request.Password, result.Value.Id.ToString(), result.Value.Code);

        await _tenantAppService.AddTenant(createTenantDto, result.Value.Id);
        await _tenantAppService.AddUser(GetCreateUserDto(request), userId);
        return new TenantDto(result.Value);
    }

    private async Task CreateUser(CreateTenantCommand request, Guid userId)
    {
        var user = new User()
        {
            Id = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Username = request.Email,
            Password = request.Password,
            Type = UserType.TenantOwner
        };
        await _userRepository.Create(user);
    }

    private static CreateTenantDto GetCreateTenantDto(CreateTenantCommand request)
    {
        return new CreateTenantDto()
        {
            Code = request.Code,
            Country = request.Country,
            City = request.City,
            Name = request.Name,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password
        };
    }

    private static CreateUserDto GetCreateUserDto(CreateTenantCommand request)
    {
        return new CreateUserDto()
        {
            Username = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password
        };
    }
}