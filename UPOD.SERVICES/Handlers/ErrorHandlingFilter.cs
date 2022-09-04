using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Reso.Core.Custom;
using System.Net;

namespace UPOD.SERVICES.Handlers
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ErrorHandlingFilter> _logger;

        public ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {

            if (context.Exception is System.Linq.Dynamic.Core.Exceptions.ParseException || context.Exception is ErrorResponse)
            {
                string message = context.Exception.ToString();
                if (context.Exception.GetType() == typeof(ErrorResponse)) message = ((ErrorResponse)context.Exception).Error.Message;
                context.Result = new ObjectResult(new ErrorResponse(((ErrorResponse)context.Exception).Error.Code, message))
                {
                    StatusCode = ((ErrorResponse)context.Exception).Error.Code,
                };
                context.ExceptionHandled = true;
                return;
            }
            _logger.LogError(context.Exception.ToString());
#if DEBUG
            context.Result = new ObjectResult(new ErrorResponse((int)HttpStatusCode.InternalServerError, context.Exception.StackTrace))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            context.ExceptionHandled = true;
#else       
            context.Result = new ObjectResult(new ErrorResponse((int)HttpStatusCode.InternalServerError, "Opps, something went wrong!"))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            context.ExceptionHandled = true;
#endif

        }
    }
}
