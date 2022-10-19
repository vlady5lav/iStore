using Infrastructure.Configurations;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services;

public class InternalHttpClientService : IInternalHttpClientService
{
    private readonly AuthorizationConfig _authConfig;

    private readonly ClientConfig _clientConfig;

    private readonly IHttpClientFactory _clientFactory;

    public InternalHttpClientService(
        IOptions<AuthorizationConfig> authConfig,
        IOptions<ClientConfig> clientConfig,
        IHttpClientFactory clientFactory)
    {
        _authConfig = authConfig.Value;
        _clientConfig = clientConfig.Value;
        _clientFactory = clientFactory;
    }

    public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
    {
        var client = _clientFactory.CreateClient();

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = $"{_authConfig.Authority}/connect/token",
                ClientId = _clientConfig.Id,
                ClientSecret = _clientConfig.Secret,
            });

        client.SetBearerToken(tokenResponse.AccessToken);

        var httpMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(url),
            Method = method,
        };

        if (content != null)
        {
            httpMessage.Content =
                new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

        var result = await client.SendAsync(httpMessage);

        if (result.IsSuccessStatusCode)
        {
            var resultContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
            return response!;
        }

        return default!;
    }
}
