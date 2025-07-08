using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;

namespace CludeMedSync.Service.Interfaces
{
	public interface IAuthService
	{
		Task<ResultadoOperacao<object>> RegisterAsync(RegisterRequest RegisterRequest);
		Task<ResultadoOperacao<TokenResponse>> LoginAsync(LoginRequest loginDto);
	}
}
