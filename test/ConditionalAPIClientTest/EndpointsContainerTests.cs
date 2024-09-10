using ConditionalAPIClient.Models;

namespace ConditionalAPIClientTest
{
    public class EndpointsContainerTests
    {
        EndpointsContainer _endpointsContainer = new EndpointsContainer();
        public EndpointsContainerTests()
        {
            var endpoints = new List<Endpoint>
            {
                new Endpoint { Id = 1, Value = "/v2/schedule" },
                new Endpoint { Id = 2, Value = "/v2/schedule_expanded" },
                new Endpoint { Id = 3, Value = "33333" },
                new Endpoint { Id = 4 },
            };
            _endpointsContainer.AddRange(endpoints);
        }

        [Fact]
        public void GetEndpointById_ShouldReturnCorrectEndpoint()
        {
            var endpoint = _endpointsContainer.GetEndpointById(2);
            Assert.NotNull(endpoint.Value);
            Assert.Equal("/v2/schedule_expanded", endpoint.Value);
        }

        [Fact]
        public void GetAllEndpoints_ShouldReturnAllEndpoints()
        {
            var allEndpoints = _endpointsContainer.GetAllEndpoints();
            Assert.Equal(4, allEndpoints.Count());
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnTrueIfExists()
        {
            var exists = _endpointsContainer.ExistsEndpointById(4);
            Assert.True(exists);
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnFalseIfNotExists()
        {
            var exists = _endpointsContainer.ExistsEndpointById(10);
            Assert.False(exists);
        }
    }
}