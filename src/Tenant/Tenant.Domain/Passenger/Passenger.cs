using System.Text.RegularExpressions;
using Common.Entity;
using Common.Validation;

namespace Tenant.Domain.Passenger;

public class Passenger : AggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    public Guid ExpeditionId { get; set; }
    public string SeatNumber { get; set; }
    public List<Ticket.Ticket> Tickets { get; set; }
    

    public Result SetFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
            return Result.Fail("Firstname can not be empty!");

        FirstName = firstName;
        return Result.Ok();
    }
    
    public Result SetLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
            return Result.Fail("Lastname can not be empty!");

        LastName = lastName;
        return Result.Ok();
    }
    
    public Result SetIdentity(string identity)
    {
        if (string.IsNullOrEmpty(identity))
            return Result.Fail("identity can not be empty!");

        if (identity.Length != 11)
            return Result.Fail("Identity length is not 11!");
        
        if(identity.Any(char.IsAscii))
            return Result.Fail("Identity should become numeric value");

        Identity = identity;
        return Result.Ok();
    }
    
    public Result SetSeatNumber(string seatNumber)
    {
        if (string.IsNullOrEmpty(seatNumber))
            return Result.Fail("Seat no can not be empty!");

        if(seatNumber.Any(char.IsAscii))
            return Result.Fail("seatNumber should become numeric value");

        SeatNumber = seatNumber;
        return Result.Ok();
    }
}