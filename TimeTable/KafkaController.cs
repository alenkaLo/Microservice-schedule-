using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using TimeTable.Models.Entity;
using TimeTable.Services;
using Newtonsoft.Json;
using System.IO;
using Confluent.Kafka;
using System.Text.Json;


namespace TimeTable.Controllers
{
    public class KafkaController 
    {
        private readonly static ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9093",
            //SecurityProtocol = SecurityProtocol.SaslPlaintext,
            //SaslMechanism = SaslMechanism.Plain,
            //SaslUsername = "your-username",
            //SaslPassword = "your-password"
        };
        public static  async Task<DeliveryResult<Null,string>> CreateEventInKafka(string NameTopic, string JsonMessage)
        {
            var message = new Message<Null, string> { Value = JsonMessage };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
               await producer.ProduceAsync(NameTopic, message);
            }
            return null;
        }

    }
}
