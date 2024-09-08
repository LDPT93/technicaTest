using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConditionalAPIClient.Models;
using ConditionalAPIClient.Service;
public class Program
{
    private static List<Endpoint> endpoints = new List<Endpoint>();
    private static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
    static async Task Main(string[] args)
    {
        GetEndpointsToAppconfig();
        var services = new ServiceCollection();
        services.AddSingleton<IEndpointService>(provider => new EndpointService(endpoints));/*addsingleton única instancia de EndpointService*/
        var serviceProvider = services.BuildServiceProvider();
        var endpointService = serviceProvider.GetService<IEndpointService>();
        Processor(endpointService, args);
    }
    #region Methods
    private static async void Api(string[] args, IEndpointService endpointService, string input)
    {
        static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddHttpClient().AddTransient<ApiService>());
        var host = CreateHostBuilder(args).Build();
        var apiService = host.Services.GetRequiredService<ApiService>();

        var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        var endpoint = endpointService.GetEndpointById(Convert.ToInt32(input)).Value;
        var apiKey = ConfigurationManager.AppSettings["APIKey"];

        var result = await apiService.GetDataFromApiAsync(baseUrl, endpoint, apiKey);
        Console.WriteLine(result);
    }
    private static void GetEndpointsToAppconfig()
    {
        int id = 1;
        foreach (var key in appSettings.AllKeys)
        {
            if (key.StartsWith("Endpoint"))
            {
                //var id = int.Parse(key.Replace("Endpoint", ""));
                var valor = appSettings[key];
                endpoints.Add(new Endpoint { Id = id++, Value = valor });
            }
        }
    }
    private static async void Processor(IEndpointService endpointService, string[] args)
    {
        var allEndpoints = endpointService.GetAllEndpoints();
        if (allEndpoints != null && allEndpoints.Any())
        {
            foreach (var endpoint in allEndpoints)
            {
                Console.WriteLine("ID: " + endpoint.Id + "   ---->   Endpint: " + endpoint.Value);
            }
            Console.WriteLine("\n");

            bool incorrectID = true;
            while (incorrectID)
            {
                Console.WriteLine("Please, enter the endpoint ID you want to use:");

                var input = Console.ReadLine();

                if (int.TryParse(input, out int number))
                {
                    if (endpointService.ExistsEndpointById(Convert.ToInt32(input)))
                    {
                        Api(args, endpointService, input);
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
            Console.WriteLine("There are no endpoints configured");
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
    #endregion

}