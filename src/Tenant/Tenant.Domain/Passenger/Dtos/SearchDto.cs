namespace Tenant.Domain.Passenger.Dtos;

public class SearchDto
{
    public string Keyword { get; set; }
    public bool IsDesc { get; set; }
    public PassengerOrderType Type { get; set; }

    public SearchDto(string keyword, PassengerOrderType type, bool isDesc)
    {
        Keyword = keyword;
        Type = type;
        IsDesc = isDesc;
    }
}

public enum PassengerOrderType
{
    FirstName,
    LastName,
    DeparturePoint,
    DepartureDate,
    ExpeditionNo
}