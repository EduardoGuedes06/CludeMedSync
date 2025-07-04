using System.ComponentModel.DataAnnotations;

namespace CludeMedSync.Service.DTOs
{
	public record RegisterDto
	{

		[Required(ErrorMessage = "O campo Email é obrigatório.")]
		[EmailAddress(ErrorMessage = "O formato do email é inválido.")]
		public string Email { get; init; } = string.Empty;

		[Required(ErrorMessage = "O campo Password é obrigatório.")]
		public string Password { get; init; } = string.Empty;

		[Required(ErrorMessage = "O campo de confirmação da Password é obrigatório.")]
		[Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
		public string ConfirmPassword { get; init; } = string.Empty;

	}


	public record LoginDto
	{
		[Required(ErrorMessage = "O campo Email é obrigatório.")]
		[EmailAddress(ErrorMessage = "O formato do email é inválido.")]
		public string Email { get; init; } = string.Empty;

		[Required(ErrorMessage = "O campo Password é obrigatório.")]
		public string Password { get; init; } = string.Empty;

	}

	public record TokenResponseDto(

		string AccessToken,
		string RefreshToken

	);

	public record RefreshTokenDto(

		[Required(ErrorMessage = "O Refresh Token é obrigatório.")]
		string RefreshToken

	);
}
