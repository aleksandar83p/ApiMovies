using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiMovies.Filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }      

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
            return base.OnExceptionAsync(context);
        }
    }
}
