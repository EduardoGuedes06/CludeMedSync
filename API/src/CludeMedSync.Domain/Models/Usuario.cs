using Microsoft.AspNetCore.Identity;

namespace CludeMedSync.Domain.Models
{
	public class Usuario : IdentityUser<string>
	{
		public string Role { get; set; } = "User";

	}
}