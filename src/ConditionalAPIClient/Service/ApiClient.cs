using ConditionalAPIClient.Models;
using Microsoft.Extensions.Options;

namespace ConditionalAPIClient.Service
{
    public class ApiClient(IHttpClientFactory httpClientFactory, IOptions<APIConfig> configuration) : IApiClient
    {
        public readonly APIConfig _apiConfig = configuration.Value;
        public readonly HttpClient _httpClient = httpClientFactory.CreateClient();

        public async Task<string> GetSchedule(string endpoint)
        {
            try
            {
                var uri = $"{_apiConfig.BaseUrl}{endpoint}?token={_apiConfig.APIKey}";
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}