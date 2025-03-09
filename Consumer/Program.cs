using Confluent.Kafka;
using Consumer.Models;
using System;
using System.Text.Json;

namespace MyApp
{
    internal class Program
    {
        private static string _bootstrapServers = "localhost:9092";
        private static string _topic = "test-topic";
        private static string _groupId = "test-group";

        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);
            Console.WriteLine("Listening for messages...");
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume();
                    var todo = JsonSerializer.Deserialize<Todo>(consumeResult.Message.Value);

                    Console.WriteLine($"Received Todo: {todo.Title}, Completed: {todo.IsCompleted}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error: {e.Error.Reason}");
                }
            }
        }
    }
}