using System.Text;
using System.Text.Json;
using Master.Application.HttpServices;
using Master.Application.HttpServices.RequestModels;
using Master.Application.HttpServices.ResponseModels;
using Microsoft.Extensions.Options;

namespace Master.Infrastructure.Keycloak;

public class KeycloakHttpService : IIdentityHttpService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly KeycloakIdentitySettings _keycloakIdentitySettings;
    private const string GroupName = "BackOffice";
    private static Dictionary<string, string> Groups => new() { { GroupName, "c203d2dd-85f9-491b-af7d-5410ac2c8e6f" } };

    public KeycloakHttpService(IHttpClientFactory httpFactory,
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
        var users = await GetUser(requestModel.Email, client);
        var groupId = Groups.FirstOrDefault(x => x.Key.Equals(GroupName)).Value;

        await SetUserPassword(client, token, password, users.First().Id);
        await AddGroup(client, token, groupId, users.First().Id);
    }

    private async Task SetUserPassword(HttpClient client, string token, string password, string userId)
    {
        var url = $"{_keycloakIdentitySettings.UserEndPoint}/{userId}/reset-password";

        var putUserRequest = new HttpRequestMessage(HttpMethod.Put, url);
        putUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        var setPasswordRequest = new PasswordRequestModel() { Value = password };
        putUserRequest.Content =
            new StringContent(JsonSerializer.Serialize(setPasswordRequest), Encoding.UTF8, "application/json");

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

    private async Task CreateUser(CreateUserRequest requestModel, HttpClient client, string token)
    {
        var url = $"{_keycloakIdentitySettings.BaseUrl}/{_keycloakIdentitySettings.UserEndPoint}";
        var createUserRequest = new HttpRequestMessage(HttpMethod.Post, url);

        createUserRequest.Headers.Add("Authorization", $"Bearer {token}");
        createUserRequest.Content =
            new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
        var result = await client.SendAsync(createUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not create user in Keycloak Identity System");
    }

    private async Task<List<UserResponseModel>> GetUser(string email, HttpClient client)
    {
        var token = await GetToken(client);
        var url = $"{_keycloakIdentitySettings.UserEndPoint}/?email={email}";
        var getUserRequest = new HttpRequestMessage(HttpMethod.Get, url);
        getUserRequest.Headers.Add("Authorization", $"Bearer {token}");

        var result = await client.SendAsync(getUserRequest);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not get user in Keycloak Server!");

        var contentAsString = await result.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(contentAsString))
            throw new Exception("Could not find any user with this email in Keycloak Server");


        return JsonSerializer.Deserialize<List<UserResponseModel>>(contentAsString);
    }

    private async Task<string> GetToken(HttpClient client)
    {
        var body = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new("grant_type", "password"),
            new("client_id", "admin-cli"),
            new("username", _keycloakIdentitySettings.Username),
            new("password", _keycloakIdentitySettings.Password),
        });
        var result = await client.PostAsync(_keycloakIdentitySettings.AdminLoginEndPoint, body);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not login to Keycloak Identity System");

        var loginResponse = JsonSerializer.Deserialize<TokenResponseModel>(await result.Content.ReadAsStringAsync());
        return loginResponse.AccessToken;
    }
}