using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAPIClient.Service
{
    public class Client : IClient
    {
        private readonly HttpClient _httpClient;
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> ApiRequest(string baseURL, string endpoint, string apiKey)
        {

            var fullURL = $"{baseURL}{endpoint}?token={apiKey}";
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
    }
}
