using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;



public class KafkaModule : IDisposable
{
    private List<Task> _taskList = new();
    private readonly ProducerConfig _producerConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9092",
    };
    public async Task<JsonResult> CreateEventInKafka(string NameTopic, string JsonMessage)
    {
        var message = new Message<Null, string> { Value = JsonMessage };
        using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
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
    public void Dispose()
    {
        _taskList.ForEach(x => x.Dispose());
    }
}

