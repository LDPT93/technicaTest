using ConditionalAPIClient.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


public class ApiClient : IApiClient
{
    public readonly APIconfig _apiConfig;
    public readonly HttpClient _httpClient;

    public ApiClient(IHttpClientFactory httpClientFactory, IOptions<APIconfig> configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiConfig = configuration.Value;
    }
    public async Task GetSchedule(string endpint, string key)
    {
        try
        {
            //_httpClient.BaseAddress = new Uri(_apiConfig.BaseUrl);
            var endpiontKey = $"http://146.190.130.247:5011/donbest{endpint}?token={key}";
            var result = await _httpClient.GetAsync(endpiontKey);
            Console.WriteLine(result.StatusCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}