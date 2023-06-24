using Serilog;
using TravelTicket.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.RegisterAll(builder.Configuration);
var app = builder.Build();

app.AddAllEndpoints();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }
