namespace ConditionalAPIClient.Service
{
    public interface IApiClient
    {
        Task<string> GetSchedule(string endpoint);
    }
}