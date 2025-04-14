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


namespace TimeTable.Controllers
{
    public class KafkaController 
    {
        private readonly static ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };
        public static async Task<JsonResult> CreateEventInKafka(string NameTopic, string JsonMessage)
        {
            var message = new Message<Null, string> { Value = JsonMessage };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                   var result = await producer.ProduceAsync(NameTopic, message);
                   return new JsonResult("Mark given and event sent to Kafka.");
                }
                catch (ProduceException<Null, string> e)
                {
                    return new JsonResult("Failed to send message to Kafka.");
                }
            }
        }

    }
}
