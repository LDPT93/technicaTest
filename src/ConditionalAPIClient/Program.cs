using ConditionalAPIClient;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System;
using ConditionalAPIClient.Models;
using ConditionalAPIClient.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    static async Task Main(string[] args)
    {
        var endpoints = new List<Endpoint>();
        var appSettings = ConfigurationManager.AppSettings;

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
        var services = new ServiceCollection();
        services.AddSingleton<IEndpointService>(provider => new EndpointService(endpoints));/*addsingleton única instancia de EndpointService*/
        var serviceProvider = services.BuildServiceProvider();
        var endpointService = serviceProvider.GetService<IEndpointService>();

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
                        static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddHttpClient().AddTransient<ApiService>());
                        var host = CreateHostBuilder(args).Build();
                        var apiService = host.Services.GetRequiredService<ApiService>();

                        var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

                        var endpoint = endpointService.GetEndpointById(Convert.ToInt32(input)).Value;

                        //var endpoint = EndpointsContainer<string>.Endpoints[int.Parse(input) - 1].Value;
                        var apiKey = ConfigurationManager.AppSettings["APIKey"];

                        var result = await apiService.GetDataFromApiAsync(baseUrl, endpoint, apiKey);
                        Console.WriteLine(result);
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
}


//


//    
//}
////Console.WriteLine(JsonSerializer.Serialize(APIContainer.APIs));
//Console.WriteLine("test");
//}
