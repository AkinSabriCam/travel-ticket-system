using Common;
using Common.Entity;

namespace Master.Domain.User;

public class User : AggregateRoot
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public UserType Type { get; set; }
}