using Microsoft.Extensions.DependencyInjection;
using ConditionalAPIClient.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ConditionalAPIClient.Service;
using System.Threading;
using System.Threading.Tasks;
public class Program()
{
    public static IConfiguration? Configuration;
    static async Task Main(string[] args)
    {
        List<Endpoint> endpoints = new List<Endpoint>();
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        Configuration = builder.Build();
        var apiConfig = Configuration.GetSection("APIconfig").Get<APIconfig>();

        var serviceCollection = new ServiceCollection();
        ConfigureClient(serviceCollection);
        var service = serviceCollection.BuildServiceProvider();
        var client = service.GetRequiredService<IClient>();

        GetEndpointsToappsettings(endpoints, apiConfig);
        ProcessEnteredParameter(endpoints);
    }
    #region Methods
    public static void GetEndpointsToappsettings(List<Endpoint> endpoints, APIconfig apiconfig)
    {
        Console.WriteLine("List of available endpoints:");

        List<string> endpointsList = new List<string>
            {
                apiconfig.Endpoint1,
                apiconfig.Endpoint2,
            };
        int count = 0;
        foreach (var e in endpointsList)
        {
            Endpoint newEndpiont = new Endpoint { Id = count++, Value = e };
            endpoints.Add(newEndpiont);
        }
        //Console.WriteLine("Please, enter the endpoint ID you want to use:");
    }
    public static async void ProcessEnteredParameter(List<Endpoint> endpoints)
    {
        if (endpoints != null && endpoints.Any())
        {
            PrintEmpointsList(endpoints);
            bool waitForInput = true;
            while (waitForInput)
            {
                var input = Console.ReadLine();
                if (input == "EXIT")
                {
                    waitForInput = false;
                }
                if (int.TryParse(input, out int number))
                {
                    if (endpoints.Any(e => e.Id == number))
                    {
                        //RequestServiceToTheApi(endpointContainer, number, configuration, client);
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
            //var input = Console.ReadLine();
            //AddNewEndpoitToConfig(input);
            //GetEndpointsToappsettings(configuration, endpointContainer);
            //ProcessEnteredParameter(endpointContainer, configuration, client);
        }
    }
    private static async void RequestServiceToTheApi(EndpointsContainer endpointService, int input, IConfiguration configuration, IClient client)
    {
        var endpoint = endpointService.GetEndpointById(Convert.ToInt32(input)).Value;
        var apiKey = configuration["APIconfig:APIKey"];
        var baseUrl = configuration["APIconfig:BaseUrl"];
        var result = await client.ApiRequest(baseUrl, endpoint, apiKey);
        Console.WriteLine(result);
        //PrintEmpointsList(endpointService.GetAllEndpoints());
    }
    private static void PrintEmpointsList(List<Endpoint> endpoints)
    {
        Console.WriteLine("List of available endpoints:");

        foreach (var endpoint in endpoints)
        {
            Console.WriteLine("ID: " + endpoint.Id + "   ---->   Endpint: " + endpoint.Value);
        }
        Console.WriteLine("\n");

        Console.WriteLine(@"Please, enter the endpoint ID you want to use or type ""EXIT"" to exit :");
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
