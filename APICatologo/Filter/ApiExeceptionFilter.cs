using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatologo.Filter
{
    public class ApiExeceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExeceptionFilter> _logger;

        public ApiExeceptionFilter(ILogger<ApiExeceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception,"Ocorreu um erro2!");

            context.Result = new ObjectResult("Ocorreu um erro!")
            {
                // StatusCode = 500,
                StatusCode = StatusCodes.Status500InternalServerError

            };
        }
    }
}
