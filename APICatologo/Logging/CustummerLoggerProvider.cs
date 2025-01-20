using System.Collections.Concurrent;

namespace APICatologo.Logging
{
    public class CustummerLoggerProvider : ILoggerProvider
    {
         readonly CustummerLoggerProviderConfiguration  _loggerConfig;

        readonly ConcurrentDictionary<string, CustumerLogger> loggers = new ConcurrentDictionary<string, CustumerLogger>();

       

        public CustummerLoggerProvider(CustummerLoggerProviderConfiguration loggerConfig)
        {
            _loggerConfig = loggerConfig;
        }

        public ILogger CreateLogger(string categoryName)
        {
           return loggers.GetOrAdd(categoryName, name => new CustumerLogger(name, _loggerConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }

    

}
