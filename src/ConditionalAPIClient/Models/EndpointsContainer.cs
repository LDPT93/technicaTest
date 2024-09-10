using ConditionalAPIClient.Models;
public class EndpointsContainer
{
    List<Endpoint> _endpoints = new List<Endpoint>();

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
    public void AddEndpointToList(Endpoint endpoint)
    {
        _endpoints.Add(endpoint);
    }
    public void AddRange(List<Endpoint> endpointsL)
    {
        _endpoints.AddRange(endpointsL);
    }
}