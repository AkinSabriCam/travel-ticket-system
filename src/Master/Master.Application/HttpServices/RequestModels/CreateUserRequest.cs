using System.Text.Json.Serialization;

namespace Master.Application.HttpServices.RequestModels;

public class CreateUserRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("enabled")]
    public bool Enabled => true;

    [JsonPropertyName("attributes")]
    public Dictionary<string, List<string>> Attributes { get; set; }
}