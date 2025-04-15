using Confluent.Kafka;

public abstract class ConsumerBase : IHostedService
{
    private readonly IConsumer<Ignore, string> _consumer;
    protected readonly IServiceScopeFactory _scopeFactory;
    protected string _topic;
    public ConsumerBase(IConsumer<Ignore, string> consumer, IServiceScopeFactory scopeFactory)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
    }
    public abstract void ProcessingMessage(string Message);
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);
        Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }
    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                ProcessingMessage(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error consuming message: {ex.Message}");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        return Task.CompletedTask;
    }
}