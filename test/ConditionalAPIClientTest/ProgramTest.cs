using Microsoft.Extensions.Configuration;
using Moq;

namespace ConditionalAPIClientTest
{
    public class ProgramTest
    {
        static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();

        [Fact]
        public void GetEndpointsToappsettings_CorrectlyLoadingAppconfigEndpoints()
        {
            var endpointsList = configuration.GetSection("APIconfig").GetChildren().Where(c => c.Key.Contains("Endpoint")).Select(c => c.Key).ToList();
            var confTest = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                {"APIconfig:Endpoint1",  "/v2/schedule"},
                {"APIconfig:Endpoint2",  "/v2/schedule_expanded"},
                {"APIconfig:Endpoint3",  "3"},
            }).Build();
            var endpointsListTest = confTest.GetSection("APIconfig").GetChildren().Where(c => c.Key.Contains("Endpoint")).Select(c => c.Key).ToList();
            Assert.NotNull(endpointsList);
            Assert.Equal(endpointsList, endpointsListTest);
        }

        [Fact]
        public void Processor_ExecutesWithoutExceptions()
        {
            var endpointService = new Mock<EndpointsContainer>();

            string[] args = new string[] { };

            var exception = Assert.Throws<InvalidOperationException>(() => Program.ProcessEnteredParameter(endpointService.Object, args, configuration));

            Assert.Null(exception);
        }
    }
}
