using Microsoft.AspNetCore.Identity;

namespace CludeMedSync.Domain.Models
{
	public class Usuario : IdentityUser<string>
	{
		public string Role { get; set; } = "User";

		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiryTime { get; set; }
	}
}