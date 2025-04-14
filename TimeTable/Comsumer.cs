using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using TimeTable.Models.Entity;
using TimeTable.Services;
using Newtonsoft.Json;
using System.IO;
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using static Confluent.Kafka.ConfigPropertyNames;


namespace TimeTable.Controllers
{
    public class Consumer 
    {
        private readonly ConsumerConfig _consumerConfig;
        public Consumer(string TopicName, string BootstrapServers, Action<string> FuctionParser, bool DeleteAfterReading = true)
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
        public void Consume(string TopicName, Action<string> FuctionParser, bool DeleteAfterReading = true)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                try
                {
                    consumer.Subscribe(TopicName);

                    while (true)
                    {
                        try
                        {
                            // Читаем сообщение
                            var consumeResult = consumer.Consume();

                            FuctionParser.Invoke(consumeResult.Message.Value);
                            // Выводим сообщение в консоль
                            Console.WriteLine($"Received message: {consumeResult.Message.Value} at {consumeResult.TopicPartitionOffset}");

                            // Здесь можно добавить обработку сообщения
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Consuming stopped.");
                }
                //finally
                //{
                //    consumer.Close();
                //}
            }
        }
    }
}
