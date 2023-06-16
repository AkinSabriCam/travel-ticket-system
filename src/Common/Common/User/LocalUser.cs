namespace Common.User;

public class LocalUser : IUser
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string TenantCode { get; set; }
    public bool IsMemberOfTheGroup { get; set; } = true;

    public bool IsAuthenticated()
    {
        throw new NotImplementedException();
    }
}