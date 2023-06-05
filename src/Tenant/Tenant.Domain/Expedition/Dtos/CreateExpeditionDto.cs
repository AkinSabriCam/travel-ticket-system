namespace Tenant.Domain.Expedition.Dtos;

public class CreateExpeditionDto
{
    public string VehicleNo { get; set; }
    public string DeparturePoint { get; set; }
    public string ArrivalPoint { get; set; }
    public DateTime DepartureDate { get; set; }
    public decimal UnitPrice { get; set; }
    public int SeatCount { get; set; }

}