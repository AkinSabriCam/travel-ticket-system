using System.Text.Json.Serialization;

namespace Master.Application.HttpServices.ResponseModels;

public class UserResponseModel
{
  [JsonPropertyName("id")]  public string Id { get; set; }
}