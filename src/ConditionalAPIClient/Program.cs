using Microsoft.Extensions.DependencyInjection;
using ConditionalAPIClient.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ConditionalAPIClient.Service;
public class Program()
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        EndpointsContainer endpointContainer = new EndpointsContainer();

        var serviceCollection = new ServiceCollection();
        ConfigureClient(serviceCollection);
        var service = serviceCollection.BuildServiceProvider();
        var client = service.GetRequiredService<IClient>();

        GetEndpointsToappsettings(configuration, endpointContainer);
        ProcessEnteredParameter(endpointContainer, configuration, client);
    }
    #region Methods
    public static void GetEndpointsToappsettings(IConfiguration configuration, EndpointsContainer endpointsContainer)
    {
        var endpointsList = configuration.GetSection("APIconfig").GetChildren().Where(c => c.Key.Contains("Endpoint")).Select(c => c.Key).ToList();
        int id = 1;
        foreach (var endpoint in endpointsList)
        {
            var apiConfig = "APIconfig";
            var key = endpoint;
            var endpintValue = configuration[$"{apiConfig}:{key}"];
            endpointsContainer.AddEndpointToList(new Endpoint { Id = id++, Value = endpintValue });
        }
    }
    public static async void ProcessEnteredParameter(EndpointsContainer endpointContainer, IConfiguration configuration, IClient client)
    {
        var allEndpoints = endpointContainer.GetAllEndpoints();
        if (allEndpoints != null && allEndpoints.Any())
        {
            PrintEmpointsList(allEndpoints);
            while (true)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int number))
                {
                    if (endpointContainer.ExistsEndpointById(Convert.ToInt32(number)))
                    {

                        RequestServiceToTheApi(endpointContainer, number, configuration, client);
                    }
                    else
                    {
                        Console.WriteLine($"The number {number}, does not match any of the available IDs.");
                    }
                }
                else
                {
                    Console.WriteLine("The entry is not a valid number.");
                }
            }
        }
        else
        {
            Console.WriteLine("There are no endpoints configured, please, insert at least one...");
            var input = Console.ReadLine();
            AddNewEndpoitToConfig(input);
            GetEndpointsToappsettings(configuration, endpointContainer);
            ProcessEnteredParameter(endpointContainer, configuration, client);
        }
    }
    private static async void RequestServiceToTheApi(EndpointsContainer endpointService, int input, IConfiguration configuration, IClient client)
    {
        var endpoint = endpointService.GetEndpointById(Convert.ToInt32(input)).Value;
        var apiKey = configuration["APIconfig:APIKey"];
        var baseUrl = configuration["APIconfig:BaseUrl"];
        var result = await client.ApiRequest(baseUrl, endpoint, apiKey);
        Console.WriteLine(result);
        PrintEmpointsList(endpointService.GetAllEndpoints());
    }
    private static void PrintEmpointsList(IEnumerable<Endpoint> allEndpoints)
    {
        Console.WriteLine("List of available endpoints:");

        foreach (var endpoint in allEndpoints)
        {
            Console.WriteLine("ID: " + endpoint.Id + "   ---->   Endpint: " + endpoint.Value);
        }
        Console.WriteLine("\n");

        Console.WriteLine("Please, enter the endpoint ID you want to use:");
    }
    private static void AddNewEndpoitToConfig(string input)
    {
        string newParameter = "Endpoint1";
        string newValue = input;
        var json = File.ReadAllText("appsettings.json");
        var jsonObj = JObject.Parse(json);
        jsonObj["APIconfig"][newParameter] = newValue;
        File.WriteAllText("appsettings.json", jsonObj.ToString());
    }
    private static void ConfigureClient(ServiceCollection services)
    {
        services.AddHttpClient<IClient, Client>();
    }
    #endregion

}
