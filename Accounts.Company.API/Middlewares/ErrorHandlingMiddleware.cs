using CORE2.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Accounts.Company.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string result = JsonConvert.SerializeObject(new { error = exception.Message });
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
           

            if (exception is ResourceNotFoundException) code = HttpStatusCode.NotFound;

            else if(exception is ValidationException validationException)
            {
                code = HttpStatusCode.BadRequest;
                result = HandleValidationException(validationException);
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private string HandleValidationException(ValidationException validationException)
        {
            string result;
            var modelErrors = new Dictionary<string, List<string>>();
            foreach (var item in validationException.Errors)
            {
                if (!modelErrors.TryGetValue(item.PropertyName, out List<string> errorMessages))
                {
                    errorMessages = new List<string>();
                    modelErrors.Add(item.PropertyName, errorMessages);
                }
                errorMessages.Add(item.ErrorMessage);

            }
            result = JsonConvert.SerializeObject(modelErrors);
            return result;
        }
    }
}
