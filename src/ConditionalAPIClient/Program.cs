using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConditionalAPIClient.Models;
using ConditionalAPIClient.Service;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;


public class Program()
{
    private static List<Endpoint> endpoints = new List<Endpoint>();
    static async Task Main(string[] args)
    {
        var configuration = GetEndpointsToappsettings();

        var services = new ServiceCollection();
        services.AddSingleton<IEndpointService>(provider => new EndpointService(endpoints));
        var serviceProvider = services.BuildServiceProvider();
        var endpointService = serviceProvider.GetService<IEndpointService>();

        Processor(endpointService, args, configuration);
    }
    #region Methods
    private static IConfiguration GetEndpointsToappsettings()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        var endpointsList = configuration.GetSection("APIconfig").GetChildren().Where(c => c.Key.Contains("Endpoint")).Select(c => c.Key).ToList();
        int id = 1;
        foreach (var endpoint in endpointsList)
        {
            var apiConfig = "APIconfig";
            var key = endpoint;
            var endpintValue = configuration[$"{apiConfig}:{key}"];
            endpoints.Add(new Endpoint { Id = id++, Value = endpintValue });
        }
        return configuration;
    }
    private static async void Processor(IEndpointService endpointService, string[] args, IConfiguration configuration)
    {
        var allEndpoints = endpointService.GetAllEndpoints();
        if (allEndpoints != null && allEndpoints.Any())
        {
            EmpointsTolist(allEndpoints);
            bool incorrectID = true;
            while (incorrectID)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int number))
                {
                    if (endpointService.ExistsEndpointById(Convert.ToInt32(input)))
                    {
                        Api(args, endpointService, input, configuration);
                    }
                    else
                    {
                        Console.WriteLine($"The number {input}, does not match any of the available IDs.");
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
            GetEndpointsToappsettings();
            Processor(endpointService, args, configuration);            
        }
    }
    private static async void Api(string[] args, IEndpointService endpointService, string input, IConfiguration configuration)
    {
        static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddHttpClient().AddTransient<ApiService>());
        var host = CreateHostBuilder(args).Build();
        var apiService = host.Services.GetRequiredService<ApiService>();

        var baseUrl = configuration["APIconfig:BaseUrl"];
        var endpoint = endpointService.GetEndpointById(Convert.ToInt32(input)).Value;
        var apiKey = configuration["APIconfig:APIKey"];

        var result = await apiService.GetDataFromApiAsync(baseUrl, endpoint, apiKey);
        Console.WriteLine(result);
        EmpointsTolist(endpointService.GetAllEndpoints());
    }
    private static void EmpointsTolist(IEnumerable<Endpoint> allEndpoints)
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
    #endregion

}
