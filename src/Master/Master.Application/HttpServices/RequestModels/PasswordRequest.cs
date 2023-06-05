using System.Text.Json.Serialization;

namespace Master.Application.HttpServices.RequestModels;

public class PasswordRequestModel
{
    [JsonPropertyName("type")]
    public string Type => "password";
    
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}