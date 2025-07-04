using CludeMedSync.Domain.Models.Exeptions;
using CludeMedSync.Service.Common;
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

			var problemDetails = CreateErroPadronizado(context, exception);

			var jsonResponse = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			});

			context.Response.ContentType = "application/problem+json";
			context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(jsonResponse);
		}

		private ResultadoOperacao<object> CreateErroPadronizado(HttpContext context, Exception exception)
		{
			var mensagemPadrao = "Ocorreu um erro inesperado ao processar sua requisição.";
			object? dados = null;

			switch (exception)
			{
				case ValidationException validationException:
					mensagemPadrao = "Uma ou mais regras de negócio não foram atendidas.";
					if (validationException.Errors.Any())
					{
						dados = new { errors = validationException.Errors };
					}
					break;

				case KeyNotFoundException:
					mensagemPadrao = "Recurso não encontrado.";
					break;

				case InvalidOperationException or ArgumentException:
					mensagemPadrao = exception.Message;
					break;

				default:
					if (_env.IsDevelopment())
					{
						dados = new { trace = exception.StackTrace };
					}
					break;
			}

			return ResultadoOperacao<object>.Falha(mensagemPadrao, dados);
		}

	}
}
