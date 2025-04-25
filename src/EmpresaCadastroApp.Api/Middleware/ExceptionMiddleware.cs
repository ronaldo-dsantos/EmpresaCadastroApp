using EmpresaCadastroApp.Application.Utils;
using System.Net;
using System.Text.Json;

namespace EmpresaCadastroApp.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = Result<string>.Fail("Ocorreu um erro inesperado. Tente novamente mais tarde.");

            var json = JsonSerializer.Serialize(result);
            return context.Response.WriteAsync(json);
        }
    }
}
