namespace TimeTable.Logging;

public static class ConsoleLogger
{
    private static ILogger _logger;
    public static ILogger Logger { get { return _logger; } }

    public static void SetLogger(ILogger logger)
    {
        _logger = logger;
    }
}
