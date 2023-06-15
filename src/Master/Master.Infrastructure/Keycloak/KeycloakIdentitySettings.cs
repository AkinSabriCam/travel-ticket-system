namespace Master.Infrastructure.Keycloak;

public class KeycloakIdentitySettings
{
    public string BaseUrl { get; set; }
    public string Realm { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string UserEndPoint { get; set; }
    public string AdminLoginEndPoint { get; set; }
    public string Audience { get; set; }
}