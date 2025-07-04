using CludeMedSync.Domain.Models;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CludeMedSync.Service.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<Usuario> _userManager;
	private readonly JwtSettings _jwtSettings;

	public AuthService(UserManager<Usuario> userManager, IConfiguration configuration)
	{
		_userManager = userManager;
		_jwtSettings = configuration.GetSection("AppSettings").Get<JwtSettings>();

		if (string.IsNullOrEmpty(_jwtSettings?.Secret))
		{
			throw new InvalidOperationException("A chave secreta do JWT (AppSettings:Secret) não foi encontrada ou está vazia no appsettings.json. Verifique a configuração.");
		}
	}

	public async Task<ResultadoOperacao<object>> RegisterAsync(RegisterDto registerDto)
	{
		var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
		if (userExists != null)
		{
			return ResultadoOperacao<object>.Falha("Já existe uma conta com este endereço de e-mail.");
		}

		var user = new Usuario
		{
			Id = Guid.NewGuid().ToString(),
			UserName = registerDto.Email,
			Email = registerDto.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
			Role = "User"
		};

		var result = await _userManager.CreateAsync(user, registerDto.Password);

		if (!result.Succeeded)
		{
			var errors = string.Join("\n", result.Errors.Select(e => e.Description));
			return ResultadoOperacao<object>.Falha($"Erro ao criar usuário: {errors}");
		}

		return ResultadoOperacao<object>.Ok("Usuário registrado com sucesso.");
	}

	public async Task<ResultadoOperacao<TokenResponseDto>> LoginAsync(LoginDto loginDto)
	{
		var user = await _userManager.FindByEmailAsync(loginDto.Email);
		if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
		{
			return ResultadoOperacao<TokenResponseDto>.Falha("E-mail ou palavra-passe inválidos.");
		}

		var accessToken = GenerateAccessToken(user);
		var refreshToken = GenerateRefreshToken();

		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

		await _userManager.UpdateAsync(user);

		var tokenResponse = new TokenResponseDto(
			AccessToken: new JwtSecurityTokenHandler().WriteToken(accessToken),
			RefreshToken: refreshToken
		);

		return ResultadoOperacao<TokenResponseDto>.Ok("Login realizado com sucesso.", tokenResponse);
	}

	private JwtSecurityToken GenerateAccessToken(Usuario user)
	{
		var authClaims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
			new(ClaimTypes.Email, user.Email),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, user.Role)
		};

		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

		var token = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
		);

		return token;
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}
}
