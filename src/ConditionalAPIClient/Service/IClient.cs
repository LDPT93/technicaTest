
namespace ConditionalAPIClient.Service
{
    public interface IClient
    {
        Task<string> ApiRequest();
    }
}