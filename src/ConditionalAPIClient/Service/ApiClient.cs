public class ApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> RequestApi(string baseUrl, string endpoint, string apiKey)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BackEnd");
            var endpointApiKey = $"{baseUrl}{endpoint}?token={apiKey}";
            var result = await client.GetAsync(endpointApiKey);

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
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }
}