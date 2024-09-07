using ConditionalAPIClient;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System;
public class Program
{
    static async Task Main(string[] args)
    {
        var _endpointsContainer = new EndpointsContainer<string>();

        var appSettings = ConfigurationManager.AppSettings;
        int _endpointid = 1;
        foreach (var key in appSettings.AllKeys)
        {
            if (key.Contains("Endpoint"))
            {
                var newEndpoint = new Endpoint<string>(_endpointid, appSettings[key]);
                _endpointsContainer.AddEndpoint(newEndpoint);
            }
            _endpointid++;
        }
        if (EndpointsContainer<string>.Endpoints.Count.Equals(0))
        {
            Console.WriteLine("There are no endpoints configured");
        }
        else
        {
            Console.WriteLine("List of available endpoints:");

            for (int i = 0; i < EndpointsContainer<string>.Endpoints.Count; i++)
            {
                Console.WriteLine("ID: " + EndpointsContainer<string>.Endpoints[i].Id + "   ---->   Endpint: " + EndpointsContainer<string>.Endpoints[i].Value);
            }
            Console.WriteLine("\n");

            bool incorrectID = true;
            while (incorrectID)
            {
                Console.WriteLine("Please, enter the endpoint ID you want to use:");

                var input = Console.ReadLine();

                if (int.TryParse(input, out int number) && _endpointsContainer.SearchId(Convert.ToInt32(input)))
                {
                    for (int i = 0; i < EndpointsContainer<string>.Endpoints.Count; i++)
                    {
                        if (number == EndpointsContainer<string>.Endpoints[i].Id)
                        {
                            //Console.WriteLine($"¡El número {number} coincide con el valor {EndpintContainer.Endpints[i].Id} en la configuración!");

                            static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddHttpClient().AddTransient<ApiService>());
                            var host = CreateHostBuilder(args).Build();
                            var apiService = host.Services.GetRequiredService<ApiService>();

                            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
                            var endpoint = EndpointsContainer<string>.Endpoints[int.Parse(input)-1].Value;
                            var apiKey = ConfigurationManager.AppSettings["APIKey"];

                            var result = await apiService.GetDataFromApiAsync(baseUrl, endpoint, apiKey);
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine($"The number {number}, does not match any of the values ​​in the configuration");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The entry is not a valid number.");
                }
            }
            //Console.WriteLine(JsonSerializer.Serialize(APIContainer.APIs));
            Console.WriteLine("test");
        }
    }
}