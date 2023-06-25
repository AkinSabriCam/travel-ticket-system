namespace Tenant.IntegrationTest.Utility;

public class TestStaticFields
{
    public static string TenantCode { get; } = "TETEN";
    public static Guid TenantId { get; } = Guid.NewGuid();
    public static Guid UserId { get; } = Guid.NewGuid();
}