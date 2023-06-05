using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Master.Application.HttpServices;
using Master.Application.HttpServices.RequestModels;
using Master.Application.HttpServices.ResponseModels;
using Microsoft.Extensions.Options;

namespace Master.Infrastructure.HttpServices;

public class IdentityHttpService : IIdentityHttpService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly KeycloakIdentitySettings _keycloakIdentitySettings;

    private Dictionary<string, string> Groups => new()
    {
        { "travel_ticket_system_group", "9b992d26-5ced-401f-8ec0-9a6254ef1fb9" }
    };

    public IdentityHttpService(IHttpClientFactory httpFactory,
        IOptions<KeycloakIdentitySettings> keycloakIdentitySettings)
    {
        _httpFactory = httpFactory;
        _keycloakIdentitySettings = keycloakIdentitySettings.Value;
    }

    public async Task CreateUser(CreateUserRequest requestModel, string password, string tenantId, string tenantCode)
    {
        var client = _httpFactory.CreateClient("keycloak");
        var token = await GetToken(client);

        await CreateUser(requestModel, client, token);
        var result = await GetUser(requestModel.Email, client);
        var userResponse = JsonSerializer.Deserialize<List<UserResponseModel>>(await result.Content.ReadAsStringAsync());
        var userId = userResponse.FirstOrDefault()?.Id;
        var groupId = Groups.FirstOrDefault(x => x.Key.Equals("travel_ticket_system_group")).Value;
        
        await SetUserPassword(client, token, password, userId);
        await AddGroup(client, token, groupId, userId);
    }

    private async Task SetUserPassword(HttpClient client, string token, string password, string userId)
    {
        var url = $"{_keycloakIdentitySettings.UserEndPoint}/{userId}/reset-password";

        var putUserRequest = new HttpRequestMessage(HttpMethod.Put, url);
        putUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        var setPasswordRequest = new PasswordRequestModel() { Value = password };
        putUserRequest.Content = new StringContent(JsonSerializer.Serialize(setPasswordRequest), Encoding.UTF8);
        putUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var result = await client.SendAsync(putUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not set user's password in Keycloak Identity System after creating the user");
    }
    
    private async Task AddGroup(HttpClient client, string token, string groupId, string userId)
    {
        var url = $"{_keycloakIdentitySettings.UserEndPoint}/{userId}/groups/{groupId}";
        var putUserRequest = new HttpRequestMessage(HttpMethod.Put, url);
        putUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        var result = await client.SendAsync(putUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not add user to group in Keycloak Identity System after creating the user");
    }

    private async Task<HttpResponseMessage> GetUser(string email, HttpClient client)
    {
        var token = await GetToken(client);
        var url = $"{_keycloakIdentitySettings.UserEndPoint}/?email={email}";
        var getUserRequest = new HttpRequestMessage(HttpMethod.Get, url);
        getUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        var result = await client.SendAsync(getUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not find user in Keycloak Identity System after creating the user");

        return result;
    }

    private async Task CreateUser(CreateUserRequest requestModel, HttpClient client, string token)
    {
        var url = $"{_keycloakIdentitySettings.BaseUrl}/{_keycloakIdentitySettings.UserEndPoint}";
        var createUserRequest = new HttpRequestMessage(HttpMethod.Post, url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        createUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        createUserRequest.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8);
        createUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var result = await client.SendAsync(createUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not create user in Keycloak Identity System");
    }

    private async Task<string> GetToken(HttpClient client)
    {
        var body = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new("username", "admin"),
            new("password", "admin"),
            new("grant_type", "password"),
            new("client_id", "admin-cli"),
        });
        var result = await client.PostAsync(_keycloakIdentitySettings.AdminLoginEndPoint, body);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not login to Keycloak Identity System");

        var loginResponse = JsonSerializer.Deserialize<TokenResponseModel>(await result.Content.ReadAsStringAsync());
        return loginResponse.AccessToken;
    }
}