using System;
using Challenge.Core.Exceptions;
using Challenge.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Challenge.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var response = new BaseResponse<bool> { HasError = true, Message = context.Exception.Message };
                context.Result = new OkObjectResult(response);
            }
            else if (context.Exception is UnauthorizedAccessException)
                context.Result = new UnauthorizedResult();
            else if (context.Exception is NotFoundException)
                context.Result = new NotFoundResult();
        }
    }
}