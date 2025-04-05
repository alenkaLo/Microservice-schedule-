using Confluent.Kafka;
using System.Threading;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TimeTable.Controllers
{
    public class TaskConsumer : IDisposable
    {
        private readonly ConsumerConfig _config;
        private readonly string _topic;
        private readonly IConsumer<Ignore, string> _consumer;
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
                        Console.WriteLine($"Received message: {package.Message.Value}");
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

    

