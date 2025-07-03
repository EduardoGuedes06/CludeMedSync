using CludeMedSync.Domain.Models.Exeptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CludeMedSync.Api.Extensions
{
	public class GlobalExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
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

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			_logger.LogError(exception, "Ocorreu uma exceção não tratada para a requisição {Path}", context.Request.Path);

			var problemDetails = CreateProblemDetails(context, exception);

			var jsonResponse = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			});

			context.Response.ContentType = "application/problem+json";
			context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(jsonResponse);
		}

		private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
		{
			var problemDetails = new ProblemDetails
			{
				Instance = context.Request.Path,
				Status = (int)HttpStatusCode.InternalServerError,
				Title = "Erro interno inesperado.",
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
				Detail = "Ocorreu um erro inesperado ao processar sua requisição."
			};

			switch (exception)
			{
				case ValidationException validationException:
					problemDetails.Status = (int)HttpStatusCode.BadRequest;
					problemDetails.Title = "Erro de validação.";
					problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
					problemDetails.Detail = "Uma ou mais regras de negócio não foram atendidas.";
					if (validationException.Errors.Any())
					{
						problemDetails.Extensions["errors"] = validationException.Errors;
					}
					break;

				case KeyNotFoundException:
					problemDetails.Status = (int)HttpStatusCode.NotFound;
					problemDetails.Title = "Recurso não encontrado.";
					problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
					problemDetails.Detail = exception.Message;
					break;

				case InvalidOperationException or ArgumentException:
					problemDetails.Status = (int)HttpStatusCode.BadRequest;
					problemDetails.Title = "Requisição inválida.";
					problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
					problemDetails.Detail = exception.Message;
					break;
			}

			if (_env.IsDevelopment())
			{
				problemDetails.Extensions["trace"] = exception.StackTrace;
			}

			return problemDetails;
		}
	}
}
