using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IAuthService
	{
		Task<ResultadoOperacao<object>> RegisterAsync(RegisterDto registerDto);
		Task<ResultadoOperacao<TokenResponseDto>> LoginAsync(LoginDto loginDto);
	}
}
