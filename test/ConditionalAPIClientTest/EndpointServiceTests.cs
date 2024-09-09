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
            new Endpoint { Id = 1, Value = "/v2/schedule" },
            new Endpoint { Id = 2, Value = "/v2/schedule_expanded" },
            new Endpoint { Id = 3, Value = "33333" },       
            new Endpoint { Id = 4 },
        };
            _endpointService = new EndpointService(endpoints);
        }

        [Fact]
        public void GetEndpointById_ShouldReturnCorrectEndpoint()
        {
            var endpoint = _endpointService.GetEndpointById(4);
            Assert.NotNull(endpoint.Value);
            Assert.Equal("/v2/schedule_expanded", endpoint.Value);
        }

        [Fact]
        public void GetAllEndpoints_ShouldReturnAllEndpoints()
        {
            var allEndpoints = _endpointService.GetAllEndpoints();
            Assert.Equal(5, allEndpoints.Count());            
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnTrueIfExists()
        {
            var exists = _endpointService.ExistsEndpointById(4);
            Assert.True(exists);
        }

        [Fact]
        public void ExistsEndpointById_ShouldReturnFalseIfNotExists()
        {
            var exists = _endpointService.ExistsEndpointById(10);
            Assert.False(exists);
        }
    }
}