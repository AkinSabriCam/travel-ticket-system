namespace Master.Infrastructure.HttpServices;

public class KeycloakIdentitySettings
{
    public string BaseUrl { get; set; }
    public string Realm { get; set; }
    public string UserEndPoint { get; set; }
    public string AdminLoginEndPoint { get; set; }

}