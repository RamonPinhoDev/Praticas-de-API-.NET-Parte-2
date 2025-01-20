
namespace APICatologo.Logging
{
    public class CustumerLogger : ILogger
    {
        readonly string _loggerName;
        readonly CustummerLoggerProviderConfiguration _loggerConfig;

        public CustumerLogger(string name, CustummerLoggerProviderConfiguration loggerConfig)
        {
            _loggerConfig = loggerConfig;
            _loggerName = name;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;

        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            EscreverTextoNoArtigo(mensagem);

        }
        private void EscreverTextoNoArtigo(string mensagem)
        {
            string CaminhoArquivoLog = @"C:\Users\Ideapad 3i\Documents\Udemy\Projetos C# O\ASPNETcore\Api\70\Aula70.txt";

            using (StreamWriter stream = new StreamWriter(CaminhoArquivoLog, true))
            {

                try
                {
                    stream.WriteLine(mensagem);
                    stream.Close();
                }
                catch (Exception) { throw; }

            }
        }
    }
}
