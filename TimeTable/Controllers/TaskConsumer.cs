using Confluent.Kafka;
using System.Threading;
using TimeTable.Models.Entity;
using TimeTable.Models.Repository;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TimeTable.Controllers
{
    public class TaskConsumer : IDisposable
    {
        private readonly ConsumerConfig _config;
        private readonly string _topic;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly LessonRepository _lessonRepository;
        private bool _disposed;

        public TaskConsumer(ConsumerConfig config = null, string topic = "task")
        {
            _config = config ?? new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "my-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _topic = topic;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _consumer.Subscribe(_topic);

        }

        public async Task Consume(CancellationToken cancellationToken = default)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var package = _consumer.Consume(cancellationToken);
                        HandleMessage(package.Message.Value);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error consuming message: {e.Error.Reason}");
                    }
                }
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task HandleMessage(string message)
        {
            try
            {
                // Десериализация JSON-сообщения
                var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(message);

                // Извлечение task_id и lesson_id
                Guid taskId = Guid.Parse(jsonObject["task_id"]?.ToString());
                Guid lessonId = Guid.Parse(jsonObject["lesson_id"]?.ToString());

                var lesson = _lessonRepository.GetById(lessonId).Result;
                await _lessonRepository.Update(lessonId, lesson.SubjectId, lesson.UserId, lesson.ClassName, taskId, lesson.StartTime, lesson.EndTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _consumer?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

    }
}

    

