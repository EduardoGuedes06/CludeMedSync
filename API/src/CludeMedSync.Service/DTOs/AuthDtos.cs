using System.ComponentModel.DataAnnotations;

namespace CludeMedSync.Service.DTOs
{
	public record RegisterDto
	{
		public string Email { get; init; } = string.Empty;
		public string Password { get; init; } = string.Empty;
		public string ConfirmPassword { get; init; } = string.Empty;
	}

	public record LoginDto
	{
		public string Email { get; init; } = string.Empty;
		public string Password { get; init; } = string.Empty;
	}

	public record TokenResponseDto(
		string AccessToken,
		string RefreshToken
	);

	public record RefreshTokenDto(
		string RefreshToken
	);
}
