namespace Tenant.Domain.Expedition;

public class SearchDto
{
    public string Keyword { get; set; }
    public bool IsDesc { get; set; }
    public OrderType OrderType { get; set; }

    public SearchDto(string keyword, OrderType type, bool isDesc)
    {
        Keyword = keyword;
        OrderType = type;
        IsDesc = isDesc;
    }
}

public enum OrderType
{
    VehicleNo,
    ExpeditionNo,
    DeparturePoint,
    ArrivalPoint,
    DepartureDate
}