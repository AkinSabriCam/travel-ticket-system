namespace Tenant.Application.Queries.Expedition.GetExpeditionById;

public class ExpeditionDto
{
    public Guid Id { get; set; }
    public string ExpeditionNo { get; set; }
    public string VehicleNo { get; set; }
    public string DeparturePoint { get; set; }
    public string ArrivalPoint { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public decimal UnitPrice { get; set; }
}