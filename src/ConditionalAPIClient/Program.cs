using ConditionalAPIClient.Models;
using ConditionalAPIClient.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class Program()
{
    static async Task Main(string[] args)
    {
        (var apiClient, var apiConfig) = InjectAllServices();

        var exitSelected = false;
        while (!exitSelected)
        {
            Console.WriteLine("List of available endpoints:");
            Console.WriteLine($"Endpoint one (1): {apiConfig.Endpoint1}");
            Console.WriteLine($"Endpoint two (2): {apiConfig.Endpoint2}");
            Console.Write(@"Please, enter the endpoint ID you want to use or type ""EXIT"" to exit :");
            var input = Console.ReadLine();
            if (!input.ToLower().Equals("exit"))
            {
                if (int.TryParse(input, out int number))
                {
                    var selectedEndpoint = string.Empty;
                    switch (number)
                    {
                        case 1:
                            selectedEndpoint = apiConfig.Endpoint1;
                            break;
                        case 2:
                            selectedEndpoint = apiConfig.Endpoint2;
                            break;
                        default:
                            Console.WriteLine("You need select the option 1 or 2");
                            Console.WriteLine();
                            break;
                    }
                    if (!string.IsNullOrEmpty(selectedEndpoint))
                    {
                        var result = await apiClient.GetSchedule(selectedEndpoint);
                        Console.WriteLine(result);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("The entry is not a valid number.");
                    Console.WriteLine();
                }
            }
            else
                exitSelected = true;

        }
    }

    private static (IApiClient, APIConfig) InjectAllServices()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddTransient<IApiClient, ApiClient>();
        services.Configure<APIConfig>(configuration.GetSection("APIconfig"));
        var serviceProvider = services.BuildServiceProvider();
        var httpclient = serviceProvider.GetService<IApiClient>();
        var apiConfig = serviceProvider.GetService<IOptions<APIConfig>>();
        return (apiClient: httpclient, apiConfig: apiConfig.Value);
    }
}
