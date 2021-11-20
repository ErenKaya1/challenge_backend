using Microsoft.AspNetCore.Mvc.Filters;

namespace Challenge.Presentation.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
        }
    }
}