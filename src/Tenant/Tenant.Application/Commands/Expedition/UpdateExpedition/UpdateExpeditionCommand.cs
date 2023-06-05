using MediatR;

namespace Tenant.Application.Commands.Expedition.UpdateExpedition;

public class UpdateExpeditionCommand : IRequest
{
    public Guid Id { get; private set; }
    public string ExpeditionNo { get; set; }
    public string VehicleNo { get; set; }
    public string DeparturePoint { get; set; }
    public string ArrivalPoint { get; set; }
    public DateTime DepartureDate { get; set; }
    public decimal UnitPrice { get; set; }

    public UpdateExpeditionCommand SetId(Guid id)
    {
        Id = id;
        return this;
    }
}