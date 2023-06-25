using System.Text;
using System.Text.Json;
using Tenant.Application.Commands.Expedition.CreateExpedition;
using Xunit;

namespace Tenant.IntegrationTest.Controller;

public class ExpeditionEpTests : BaseEpTest
{
    [Fact]
    public async Task should_success_when_create_expedition()
    {
        var command = new CreateExpeditionCommand()
        {
            ArrivalPoint = "ACity",
            DeparturePoint = "BCity",
            DepartureDate = DateTime.Now,
            VehicleNo = "34XXX321",
            UnitPrice = 500,
            SeatCount = 30
        };

        var result = await Client.PostAsync("api/expedition",
            new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));

        Assert.True(result.IsSuccessStatusCode);
    }
}