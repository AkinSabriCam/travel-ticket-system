namespace Common.User;

public interface IUser
{
    Guid UserId {get; set;}
    Guid TenantId { get; set; }
    string TenantCode { get; set; }

    bool IsAuthenticated();
}