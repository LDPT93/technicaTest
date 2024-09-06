using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Models;
using System.Text.Json;
//using Shared.Models;

namespace EncrypterDateProducer
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
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var currentTimeSerialized = SHA268generator.CurrentTimeToSha256();
                        //MessageDTO jsonSHA265Deserialized = JsonSerializer.Deserialize<MessageDTO>(currentTimeSerialized);
                        var message = new Message<Null, string> { Value = currentTimeSerialized };
                        await producer.ProduceAsync(_kafkaSettings.Topic, message, stoppingToken);/*_kafkaSettings.Topic*/
                        _logger.LogInformation("Mensaje enviado a Kafka-Redpanda a las {time}", currentTimeSerialized);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error enviando mensaje a Kafka-Redpanda");
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

    }
}

