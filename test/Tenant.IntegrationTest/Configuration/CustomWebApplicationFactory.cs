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
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tenant.Infrastructure.EfCore;
using Tenant.IntegrationTest.Utility;

namespace Tenant.IntegrationTest.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        RunComposeFile();

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
                Code = TestStaticFields.TenantCode,
                Email = "testtenant@gmail.com"
            }).Wait();
        });

        builder.ConfigureTestServices(services =>
        {
            services
                .AddAuthentication("test")
                .AddScheme<TestAuthHandlerOptions, TestAuthenticationHandler>("test", _ => { });
        });

        base.ConfigureWebHost(builder);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
        var token = GenerateJwtToken();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("test", token);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static string GenerateJwtToken()
    {
        var key = "Azxfe5vaw3R2asdzasf3"u8.ToArray();
        var token = new JwtSecurityToken(
            issuer: "http://localhost/",
            audience: "http://localhost",
            expires: DateTime.UtcNow.AddMinutes(30),
            claims: new Claim[]
            {
                new("userId", TestStaticFields.UserId.ToString()),
                new("tenantId", TestStaticFields.TenantId.ToString()),
                new("tenantCode", TestStaticFields.TenantCode),
                new("memberOf", "BackOffice"),
            },
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature));

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private static async Task CreateTestTenant(IServiceCollection services, CreateTenantDto createTenantDto)
    {
        try
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var tenantAppService = scope.ServiceProvider.GetRequiredService<ITenantAppService>();
            var tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

            await tenantDbContext.Database.MigrateAsync();
            await tenantAppService.AddTenant(createTenantDto, TestStaticFields.TenantId);

            await LocalUserContext.SetUser(new LocalUser()
            {
                TenantCode = TestStaticFields.TenantCode,
                TenantId = TestStaticFields.TenantId,
                UserId = TestStaticFields.UserId
            });

            tenantAppService = scope.ServiceProvider.GetRequiredService<ITenantAppService>();
            await tenantAppService.AddUser(new CreateUserDto()
            {
                FirstName = createTenantDto.FirstName,
                LastName = createTenantDto.LastName,
                Email = createTenantDto.Email,
                Username = createTenantDto.Email,
                Password = createTenantDto.Password,
            }, TestStaticFields.UserId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private static void RunComposeFile()
    {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = Directory.GetCurrentDirectory(),
            FileName = "cmd.exe",
            Arguments = "/c docker-compose up -d",
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExitAsync().Wait();
        Task.Delay(10000).Wait();
    }

    public override ValueTask DisposeAsync()
    {
        // Process.Start("cmd.exe", "/ docker-compose down");

        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = Directory.GetCurrentDirectory(),
            FileName = "cmd.exe",
            Arguments = "/c docker-compose down",
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExitAsync().Wait();

        return base.DisposeAsync();
    }
} 