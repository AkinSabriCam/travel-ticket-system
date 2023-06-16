using System.Globalization;
using Common.Entity;
using Common.Validation;
using Tenant.Domain.Ticket;

namespace Tenant.Domain.Expedition;

public class Expedition : AggregateRoot
{
    #region ExpeditionNp

    public string ExpeditionNo { get; private set; }

    private string _expeditionNo
    {
        set
        {
            Changes.Add(new Field(ExpeditionNo, value, "ExpeditionNo"));
            ExpeditionNo = value;
        }
    }

    #endregion

    #region VehicleNo

    public string VehicleNo { get; private set; }

    private string _vehicleNo
    {
        set
        {
            Changes.Add(new Field(VehicleNo, value, "VehicleNo"));
            VehicleNo = value;
        }
    }

    #endregion

    #region DeparturePoint

    public string DeparturePoint { get; private set; }

    private string _departurePoint
    {
        set
        {
            Changes.Add(new Field(DeparturePoint, value, "DeparturePoint"));
            DeparturePoint = value;
        }
    }

    #endregion

    #region ArrivalPoint

    public string ArrivalPoint { get; private set; }

    private string _arrivalPoint
    {
        set
        {
            Changes.Add(new Field(ArrivalPoint, value, "ArrivalPoint"));
            ArrivalPoint = value;
        }
    }

    #endregion

    #region DepartureDate

    public DateTime DepartureDate { get; private set; }

    private DateTime _departureDate
    {
        set
        {
            Changes.Add(new Field(DepartureDate.ToString(CultureInfo.InvariantCulture),
                value.ToString(CultureInfo.InvariantCulture), "DepartureDate"));
            DepartureDate = value;
        }
    }

    #endregion

    #region UnitPrice

    public decimal UnitPrice { get; private set; }

    private decimal _unitPrice
    {
        set
        {
            Changes.Add(new Field(UnitPrice.ToString(CultureInfo.InvariantCulture),
                value.ToString(CultureInfo.InvariantCulture), "UnitPrice"));
            UnitPrice = value;
        }
    }

    #endregion

    #region SeatCount

    public int SeatCount { get; private set; }

    private int _seatCount
    {
        set
        {
            Changes.Add(new Field(SeatCount.ToString(), value.ToString(), "SeatCount"));
            SeatCount = value;
        }
    }

    #endregion

    #region ExpeditionStatus

    public ExpeditionStatus Status { get; private set; } = ExpeditionStatus.Active;

    private ExpeditionStatus _status
    {
        get => Status;
        set
        {
            Changes.Add(new Field(Status.ToString(), value.ToString(), "Status"));
            Status = value;
        }
    }

    #endregion


    public List<Ticket.Ticket> Tickets { get; set; }

    public Result SetExpeditionNo(string expeditionNo)
    {
        if (string.IsNullOrWhiteSpace(expeditionNo))
            return Result.Fail("Expedition No could not be null!");

        _expeditionNo = expeditionNo;
        return Result.Ok();
    }

    public Result SetVehicleNo(string vehicleNo)
    {
        if (string.IsNullOrWhiteSpace(vehicleNo))
            return Result.Fail("Vehicle No could not be null!");

        _vehicleNo = vehicleNo;
        return Result.Ok();
    }

    public Result SetDeparturePoint(string departurePoint)
    {
        if (string.IsNullOrWhiteSpace(departurePoint))
            return Result.Fail("Departure point could not be null!");

        _departurePoint = departurePoint;
        return Result.Ok();
    }

    public Result SetArrivalPoint(string arrivalPoint)
    {
        if (string.IsNullOrWhiteSpace(arrivalPoint))
            return Result.Fail("Arrival point could not be null!");

        _arrivalPoint = arrivalPoint;
        return Result.Ok();
    }

    public Result SetDepartureDate(DateTime departureDate)
    {
        if (departureDate == DateTime.MinValue || departureDate == DateTime.MaxValue)
            return Result.Fail("DepartureDate can not be invalid date!");

        _departureDate = departureDate;
        return Result.Ok();
    }

    public Result SetUnitPrice(decimal unitPrice)
    {
        if (unitPrice is <= 0 or decimal.MaxValue)
            return Result.Fail("Unit Price can not be invalid value!");

        _unitPrice = unitPrice;
        return Result.Ok();
    }

    public Result SetSeatCount(int seatCount)
    {
        if (seatCount is <= 0)
            return Result.Fail("Seat count can not be invalid value!");

        _seatCount = seatCount;
        return Result.Ok();
    }
    
    public void SetStatus(ExpeditionStatus status)
    {
        _status = status;
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