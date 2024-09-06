using Confluent.Kafka;

namespace EncrypterDateProducer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ProducerConfig confi;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            confi = new ProducerConfig { BootstrapServers = "localhost:19092" };

        }
        ClassSHA268generator _classSHA268Generator = new ClassSHA268generator();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var producer = new ProducerBuilder<Null, string>(confi).Build())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //if (_logger.IsEnabled(LogLevel.Information))/*preguntar*/
                    //{
                    try
                    {
                        var message = new Message<Null, string> { Value = _classSHA268Generator.CurrentTimeToSha256() };
                        await producer.ProduceAsync("chat-room", message, stoppingToken);
                        _logger.LogInformation("Mensaje enviado a Kafka-Redpanda a las {time}", DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error enviando mensaje a Kafka-Redpanda");
                    }

                    //_logger.LogInformation("Worker running at: {time}", DateTime.Now);
                    //}
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
