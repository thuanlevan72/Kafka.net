using Confluent.Kafka;
using System.Text.Json;

namespace Producer.Services
{
    public class KafkaProducer
    {
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "test-topic";
        public async Task SendMessageAsync(string message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var deliveryReport = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Sent: {deliveryReport.Value} to {deliveryReport.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error: {e.Error.Reason}");
            }
        }
        public async Task SendMessageAsync<T>(T message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var jsonMessage = JsonSerializer.Serialize(message);
                var deliveryReport = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = jsonMessage });
                Console.WriteLine($"Sent: {deliveryReport.Value} to {deliveryReport.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error: {e.Error.Reason}");
            }
        }
    }
}
