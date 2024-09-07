namespace ConditionalAPIClient
{
    public class EndpointsContainer<T>
    {
        public static List<Endpoint<T>> Endpoints;
        public EndpointsContainer()
        {
            Endpoints = new List<Endpoint<T>>();
        }

        public void AddEndpoint(Endpoint<T> endpoint)
        {
            Endpoints.Add(endpoint);
        }
        public T ObtenerValorPorId(int EndpointId)
        {
            var endpoint = Endpoints.FirstOrDefault(e => e.Id == EndpointId);
            if (endpoint != null)
            {
                return endpoint.Value;
            }
            else
            {
                throw new Exception("Endpint not found.");
            }
        }

        public bool SearchId(int number)
        {
            return Endpoints.Any(e => e.Id == number);
        }

    }
}
