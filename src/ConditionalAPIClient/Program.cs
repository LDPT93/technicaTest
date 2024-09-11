using Microsoft.Extensions.DependencyInjection;
using ConditionalAPIClient.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ConditionalAPIClient.Service;
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
        ProcessEnteredParameter(endpoints, apiConfig, client);
    }
    #region Methods
    public static void GetEndpointsToappsettings(List<Endpoint> endpoints, APIconfig apiconfig)
    {
        List<string> endpointsList = new List<string>
            {
                apiconfig.Endpoint1,
                apiconfig.Endpoint2,
            };
        int count = 1;
        foreach (var e in endpointsList)
        {
            Endpoint newEndpiont = new Endpoint { Id = count++, Value = e };
            endpoints.Add(newEndpiont);
        }
        //Console.WriteLine("Please, enter the endpoint ID you want to use:");
    }
    public static async void ProcessEnteredParameter(List<Endpoint> endpoints, APIconfig apiconfig, IClient client)
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
                        var result = await client.ApiRequest(apiconfig, number, endpoints);
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
            Console.WriteLine("There are no endpoints configured");            
        }
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
    private static void ConfigureClient(ServiceCollection services)
    {
        services.AddHttpClient<IClient, Client>();
    }
    #endregion

}
