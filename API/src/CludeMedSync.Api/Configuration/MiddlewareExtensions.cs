using CludeMedSync.Api.Extensions;

namespace CludeMedSync.Api.Configuration
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
