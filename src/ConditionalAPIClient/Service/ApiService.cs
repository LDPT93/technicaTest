public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetDataFromApiAsync(string baseUrl, string endpoint, string apiKey)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            //client.BaseAddress = new Uri(baseUrl);
            var urlEndpointApiKey = $"{baseUrl}{endpoint}?token={apiKey}";
            var response = await client.GetAsync(urlEndpointApiKey);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }

    }
}