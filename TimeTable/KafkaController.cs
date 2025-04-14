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
                    await producer.ProduceAsync(NameTopic, message);
                    var result = new JsonResult("Mark given and event sent to Kafka.");
                    result.StatusCode = 200;
                    return result;
                }
                catch (ProduceException<Null, string> e)
                {
                    var result = new JsonResult("Failed to send message to Kafka.");
                    result.StatusCode = 500;
                    return result;
                }
            }
        }

    }
}
