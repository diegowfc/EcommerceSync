using Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Filters
{
    public sealed class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            var (status, title) = ex switch
            {
                EmailAlreadyExistsException => (StatusCodes.Status409Conflict, "E-mail já cadastrado"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno")
            };

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = ex.Message,
                Instance = context.HttpContext.Request.Path
            };

            context.Result = new ObjectResult(problem) { StatusCode = status };
            context.ExceptionHandled = true;
        }
    }
}
