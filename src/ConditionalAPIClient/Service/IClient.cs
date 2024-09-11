using ConditionalAPIClient.Models;

namespace ConditionalAPIClient.Service
{
    public interface IClient
    {
        Task<string> ApiRequest(APIconfig apiConfig, int endpointId, List<Endpoint> endpoints);
    }
}