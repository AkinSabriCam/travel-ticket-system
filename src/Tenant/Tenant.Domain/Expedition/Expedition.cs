using Common.Entity;
using Common.Validation;
using Tenant.Domain.Ticket;

namespace Tenant.Domain.Expedition;

public class Expedition : AggregateRoot
{
    public string ExpeditionNo { get; private set; }
    public string VehicleNo { get; private set; }
    public string DeparturePoint { get; private set; }
    public string ArrivalPoint { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int SeatCount { get; private set; }
    public List<Ticket.Ticket> Tickets { get; set; }
    public ExpeditionStatus Status { get; set; } = ExpeditionStatus.Active;

    public Result SetExpeditionNo(string expeditionNo)
    {
        if (string.IsNullOrWhiteSpace(expeditionNo))
            return Result.Fail("Expedition No could not be null!");

        ExpeditionNo = expeditionNo;
        return Result.Ok();
    }
    
    public Result SetVehicleNo(string vehicleNo)
    {
        if (string.IsNullOrWhiteSpace(vehicleNo))
            return Result.Fail("Vehicle No could not be null!");
        
        VehicleNo = vehicleNo;
        return Result.Ok();
    }
    
    public Result SetDeparturePoint(string departurePoint)
    {
        if (string.IsNullOrWhiteSpace(departurePoint))
            return Result.Fail("Departure point could not be null!");
        
        DeparturePoint = departurePoint;
        return Result.Ok();
    }
    
    public Result SetArrivalPoint(string arrivalPoint)
    {
        if (string.IsNullOrWhiteSpace(arrivalPoint))
            return Result.Fail("Arrival point could not be null!");
        
        ArrivalPoint = arrivalPoint;
        return Result.Ok();
    }
    
    public Result SetDepartureDate(DateTime departureDate)
    {
        if (departureDate == DateTime.MinValue || departureDate == DateTime.MaxValue)
            return Result.Fail("DepartureDate can not be invalid date!");

        DepartureDate = departureDate;
        return Result.Ok();
    }
    
    public Result SetUnitPrice(decimal unitPrice)
    {
        if (unitPrice is <= 0 or decimal.MaxValue)  
            return Result.Fail("Unit Price can not be invalid value!");

        UnitPrice = unitPrice;
        return Result.Ok();
    }
    
    public Result SetSeatCount(int seatCount)
    {
        if (seatCount is  <= 0)
            return Result.Fail("Seat count can not be invalid value!");

        SeatCount = seatCount;
        return Result.Ok();
    }

    public bool IsAvailableForSale()
    {
        return Tickets.Count(x => x.Status != TicketStatus.Cancelled) < SeatCount;
    }
}

public enum ExpeditionStatus
{
    Active,
    Cancelled
}