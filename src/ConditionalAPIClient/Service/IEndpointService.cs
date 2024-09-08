using ConditionalAPIClient.Models;

namespace ConditionalAPIClient.Service
{
    public interface IEndpointService
    {
        Endpoint GetEndpointById(int id);
        IEnumerable<Endpoint> GetAllEndpoints();
        bool ExistsEndpointById(int id);
    }
}
