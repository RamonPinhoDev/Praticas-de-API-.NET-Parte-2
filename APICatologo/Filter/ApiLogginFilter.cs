using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace APICatologo.Filter
{
    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger<ApiLogginFilter> _logger;

        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

            _logger.LogInformation("Executado login......");
            _logger.LogInformation("###################");
            _logger.LogInformation(DateTime.UtcNow.ToLongTimeString());

            _logger.LogInformation($"Resultado do status:{context.HttpContext.Response.StatusCode}");




        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Execundo login......");
            _logger.LogInformation("###################");
            _logger.LogInformation(DateTime.UtcNow.ToLongTimeString());
            _logger.LogInformation($"Mostrando o Model:{context.ModelState.IsValid}");
            _logger.LogInformation("###################");

        }

       
    }
}
