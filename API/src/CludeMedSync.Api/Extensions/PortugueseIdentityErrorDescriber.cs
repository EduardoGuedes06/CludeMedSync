using Microsoft.AspNetCore.Identity;

namespace CludeMedSync.Api.Extensions
{
	public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DefaultError()
			=> new() { Code = nameof(DefaultError), Description = "Ocorreu um erro desconhecido." };

		public override IdentityError ConcurrencyFailure()
			=> new() { Code = nameof(ConcurrencyFailure), Description = "Falha de concorrência otimista, o objeto foi modificado." };

		public override IdentityError PasswordMismatch()
			=> new() { Code = nameof(PasswordMismatch), Description = "Palavra-passe incorreta." };

		public override IdentityError InvalidToken()
			=> new() { Code = nameof(InvalidToken), Description = "Token inválido." };

		public override IdentityError LoginAlreadyAssociated()
			=> new() { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um utilizador com este login." };

		public override IdentityError InvalidUserName(string? userName)
			=> new() { Code = nameof(InvalidUserName), Description = $"O nome de utilizador '{userName}' é inválido. Pode conter apenas letras e números." };

		public override IdentityError InvalidEmail(string? email)
			=> new() { Code = nameof(InvalidEmail), Description = $"O e-mail '{email}' é inválido." };

		public override IdentityError DuplicateUserName(string? userName)
			=> new() { Code = nameof(DuplicateUserName), Description = $"O nome de utilizador '{userName}' já está a ser utilizado." };

		public override IdentityError DuplicateEmail(string? email)
			=> new() { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' já está a ser utilizado." };

		public override IdentityError InvalidRoleName(string? role)
			=> new() { Code = nameof(InvalidRoleName), Description = $"A função '{role}' é inválida." };

		public override IdentityError DuplicateRoleName(string? role)
			=> new() { Code = nameof(DuplicateRoleName), Description = $"A função '{role}' já está a ser utilizada." };

		public override IdentityError UserAlreadyHasPassword()
			=> new() { Code = nameof(UserAlreadyHasPassword), Description = "O utilizador já tem uma palavra-passe definida." };

		public override IdentityError UserLockoutNotEnabled()
			=> new() { Code = nameof(UserLockoutNotEnabled), Description = "O lockout não está ativo para este utilizador." };

		public override IdentityError UserAlreadyInRole(string? role)
			=> new() { Code = nameof(UserAlreadyInRole), Description = $"O utilizador já tem a função '{role}'." };

		public override IdentityError UserNotInRole(string? role)
			=> new() { Code = nameof(UserNotInRole), Description = $"O utilizador não tem a função '{role}'." };

		public override IdentityError PasswordTooShort(int length)
			=> new() { Code = nameof(PasswordTooShort), Description = $"A palavra-passe deve ter pelo menos {length} caracteres." };

		public override IdentityError PasswordRequiresNonAlphanumeric()
			=> new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A palavra-passe deve conter pelo menos um caracter não alfanumérico." };

		public override IdentityError PasswordRequiresDigit()
			=> new() { Code = nameof(PasswordRequiresDigit), Description = "A palavra-passe deve conter pelo menos um dígito ('0'-'9')." };

		public override IdentityError PasswordRequiresLower()
			=> new() { Code = nameof(PasswordRequiresLower), Description = "A palavra-passe deve conter pelo menos uma letra minúscula ('a'-'z')." };

		public override IdentityError PasswordRequiresUpper()
			=> new() { Code = nameof(PasswordRequiresUpper), Description = "A palavra-passe deve conter pelo menos uma letra maiúscula ('A'-'Z')." };
	}

}
