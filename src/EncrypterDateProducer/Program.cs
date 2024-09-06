using EncrypterDateProducer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();
        host.Run();
    }
}