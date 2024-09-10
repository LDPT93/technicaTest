using EncrypterDateProducer;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
        builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();
        host.Run();
    }
}