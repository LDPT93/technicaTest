using ConditionalAPIClient.Service;
using ConditionalAPIClient.Models;

public class EndpointService : IEndpointService
{
    private readonly List<Endpoint> _endpoints;

    public EndpointService(List<Endpoint> endpoints)
    {
        _endpoints = endpoints;
    }

    public Endpoint GetEndpointById(int id)
    {
        return _endpoints.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Endpoint> GetAllEndpoints()
    {
        return _endpoints;
    }

    public bool ExistsEndpointById(int id)
    {
        return _endpoints.Any(e => e.Id == id);
    }
}