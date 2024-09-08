using ConditionalAPIClient.Service;
using ConditionalAPIClient.Models;

namespace ConditionalAPIClientTest
{
    public class EndpointServiceTests
    {
        private readonly IEndpointService _endpointService;

        public EndpointServiceTests()
        {
            var endpoints = new List<Endpoint>
        {
            new Endpoint { Id = 1, Value = "/todos/0" },
            new Endpoint { Id = 2, Value = "/todos/1" },
            new Endpoint { Id = 3, Value = "/todos/2" },
            new Endpoint { Id = 4, Value = "/todos/3" },
            new Endpoint { Id = 5, Value = "/todos/4" }
        };
            _endpointService = new EndpointService(endpoints);
        }

        [Fact]
        public void GetEndpointById_ShouldReturnCorrectEndpoint()
        {
            var endpoint = _endpointService.GetEndpointById(1);
            Assert.NotNull(endpoint);
            Assert.Equal(1, endpoint.Id);
            Assert.Equal("/todos/0", endpoint.Value);
        }

        [Fact]
        public void GetAllEndpoints_ShouldReturnAllEndpoints()
        {
            var allEndpoints = _endpointService.GetAllEndpoints();
            Assert.NotNull(allEndpoints);
            Assert.Equal(5, allEndpoints.Count());
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnTrueIfExists()
        {
            var exists = _endpointService.ExistsEndpointById(1);
            Assert.True(exists);
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnFalseIfNotExists()
        {
            var exists = _endpointService.ExistsEndpointById(3);
            Assert.False(exists);
        }
    }
}