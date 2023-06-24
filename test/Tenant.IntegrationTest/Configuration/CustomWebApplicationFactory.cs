using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Common.User;
using Master.Application.Abstraction;
using Master.Application.Abstraction.Dto;
using Master.Application.HttpServices;
using Master.Domain.Tenant.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tenant.Infrastructure.EfCore;
using Tenant.IntegrationTest.Utility;

namespace Tenant.IntegrationTest.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();
    private const string TenantCode = "TESTEN";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = Directory.GetCurrentDirectory(),
            FileName = "cmd.exe",
            Arguments = "/c docker-compose up",
        };
        process.StartInfo = startInfo;
        process.Start();

        Task.Delay(10000).Wait();
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        builder.UseConfiguration(configuration);
        
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IIdentityHttpService, FakeIdentityService>();

            CreateTestTenant(services, new CreateTenantDto()
            {
                Name = "Test Tenant",
                FirstName = "Jane",
                LastName = "Doe",
                Country = "türkiye",
                City = "istanbul",
                Code = TenantCode,
                Email = "testtenant@gmail.com"
            }).Wait();
        });
        
        base.ConfigureWebHost(builder);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GenerateJwtToken());
    }

    private string GenerateJwtToken()
    {
        var key = "ASvAsdTe12qwy4asdawewar342fegdfdsfsdfs"u8.ToArray();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            
            // Issuer = "http://localhost:6743/realms/",
            // Audience = "http://localhost:6743/realms/",
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", _userId.ToString()),
                new Claim("tenantId", _tenantId.ToString()),
                new Claim("tenantCode", TenantCode),
                new Claim("memberOf", "BackOffice")
            })
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task CreateTestTenant(IServiceCollection services, CreateTenantDto createTenantDto)
    {
        try
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var tenantAppService = scope.ServiceProvider.GetRequiredService<ITenantAppService>();
            var tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

            await tenantDbContext.Database.MigrateAsync();
            await tenantAppService.AddTenant(createTenantDto, _tenantId);

            await LocalUserContext.SetUser(new LocalUser()
            {
                TenantCode = TenantCode,
                TenantId = _tenantId,
                UserId = _userId
            });

            tenantAppService = scope.ServiceProvider.GetRequiredService<ITenantAppService>();
            await tenantAppService.AddUser(new CreateUserDto()
            {
                FirstName = createTenantDto.FirstName,
                LastName = createTenantDto.LastName,
                Email = createTenantDto.Email,
                Username = createTenantDto.Email,
                Password = createTenantDto.Password,
            }, _userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}