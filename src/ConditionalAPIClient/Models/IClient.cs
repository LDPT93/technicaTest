namespace ConditionalAPIClient.Models
{
    public interface IClient
    {
        Task<string> ApiRequest(string baseURL, string endpoint, string apiKey);
    }
}