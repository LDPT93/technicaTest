namespace ConditionalAPIClient
{
    public class Endpoint <T>
    {
        public int Id { get; set; }
        public T Value { get; set; }

        public Endpoint(int id, T endpoint)
        {
            Id = id;
            Value = endpoint;
        }
    }
}
