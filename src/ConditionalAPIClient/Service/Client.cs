using ConditionalAPIClient.Models;
using System.Net.Http.Json;

namespace ConditionalAPIClient.Service
{
    public class Client : IClient
    {
        private readonly HttpClient _httpClient;
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> ApiRequest(APIconfig apiConfig, int endpointId, List<Endpoint> endpoints)
        {
            try
            {
                var endpintValue = endpoints.FirstOrDefault(e => e.Id == endpointId).Value;
                var fullURL = $"{apiConfig.BaseUrl}{endpintValue}?token={apiConfig.APIKey}";
                var result = await _httpClient.GetAsync(fullURL);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {result.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                throw;
            }

        }
    }
}
