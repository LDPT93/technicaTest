using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace DecrypterDateConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KafkaSettings _kafkaSettings;


        public Worker(ILogger<Worker> logger, IOptions<KafkaSettings> kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                EnableAutoCommit = false
            };

            string kafkaTopic = _kafkaSettings.Topic;

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(kafkaTopic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        _logger.LogInformation($"Message received: {consumeResult.Message.Value} at {DateTime.UtcNow}");

                        consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Error consuming Kafka message");
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
