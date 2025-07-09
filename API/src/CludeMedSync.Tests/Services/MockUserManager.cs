using CludeMedSync.Domain.Models;
using CludeMedSync.Models.Request;
using CludeMedSync.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CludeMedSync.Tests.Services
{
	public static class MockUserManager
	{
		public static Mock<UserManager<TUser>> Create<TUser>() where TUser : IdentityUser<string>
		{
			var store = new Mock<IUserStore<TUser>>();
			var userManagerMock = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);

			userManagerMock.Object.UserValidators.Add(new UserValidator<TUser>());
			userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

			return userManagerMock;
		}
	}

	public class AuthServiceTests
	{
		private readonly Mock<UserManager<Usuario>> _userManagerMock;
		private readonly AuthService _service;

		public AuthServiceTests()
		{
			_userManagerMock = MockUserManager.Create<Usuario>();

			var jwtSettings = new Dictionary<string, string>
			{
				{"AppSettings:Secret", "MINHA_CHAVE_SECRETA_SUPER_LONGA_PARA_TESTES_E_SEGURANCA"},
				{"AppSettings:Issuer", "MeuApp"},
				{"AppSettings:Audience", "MeuApp"},
				{"AppSettings:AccessTokenExpirationMinutes", "60"},
				{"AppSettings:RefreshTokenExpirationDays", "7"}
			};
			var configuration = new ConfigurationBuilder().AddInMemoryCollection(jwtSettings).Build();

			_service = new AuthService(_userManagerMock.Object, configuration);
		}

		[Fact]
		public async Task RegisterAsync_DeveRegistrarUsuario_QuandoEmailNaoExiste()
		{
			var request = new RegisterRequest { Email = "teste@email.com", Password = "PalavraPasseForte123!" };

			_userManagerMock.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((Usuario)null);
			_userManagerMock.Setup(um => um.CreateAsync(It.IsAny<Usuario>(), request.Password)).ReturnsAsync(IdentityResult.Success);

			var resultado = await _service.RegisterAsync(request);

			resultado.Sucesso.Should().BeTrue();
			resultado.Mensagem.Should().Be("Usuário registrado com sucesso.");
		}

		[Fact]
		public async Task RegisterAsync_DeveFalhar_QuandoEmailJaExiste()
		{
			var request = new RegisterRequest { Email = "existente@email.com", Password = "PalavraPasseForte123!" };

			_userManagerMock.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(new Usuario());

			var resultado = await _service.RegisterAsync(request);

			resultado.Sucesso.Should().BeFalse();
			resultado.Mensagem.Should().Be("Já existe uma conta com este endereço de e-mail.");
		}

		[Fact]
		public async Task LoginAsync_DeveRetornarToken_QuandoCredenciaisValidas()
		{
			var request = new LoginRequest { Email = "usuario@teste.com", Password = "Senha123!" };
			var user = new Usuario { Id = Guid.NewGuid().ToString(), UserName = "usuario@teste.com", Email = "usuario@teste.com", Role = "User" };

			_userManagerMock.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(user);
			_userManagerMock.Setup(um => um.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
			_userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(IdentityResult.Success);

			var resultado = await _service.LoginAsync(request);

			resultado.Sucesso.Should().BeTrue();
			resultado.Dados.Should().NotBeNull();
			resultado.Dados?.AccessToken.Should().NotBeNullOrEmpty();
			resultado.Dados?.RefreshToken.Should().NotBeNullOrEmpty();
		}
	}
}