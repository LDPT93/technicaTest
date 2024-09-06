using DecrypterDateConsumer;
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