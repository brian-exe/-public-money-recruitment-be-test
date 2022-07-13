using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(NotFoundException ex)
            {
                await HandleNotFoundException(context, ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleInternalErrorException(context, ex).ConfigureAwait(false);
            }
        }

        private Task HandleNotFoundException(HttpContext context, Exception ex)
            => HandleException(context, (int)HttpStatusCode.NotFound, ex.Message);

        private Task HandleInternalErrorException(HttpContext context, Exception ex)
        {
            var message = "An internal error has ocurred. Please try again later.";
            return HandleException(context, (int)HttpStatusCode.InternalServerError, message);
        }

        private Task HandleException(HttpContext context, int statusCode, object response, string contentType = "application/json")
        {
            context.Response.ContentType = contentType;
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
