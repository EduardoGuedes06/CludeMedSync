using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace CludeMedSync.Data.Auth;

public class DapperUserStore : IUserStore<Usuario>, IUserPasswordStore<Usuario>, IUserEmailStore<Usuario>
{
	private readonly DbConnectionFactory _connectionFactory;

	public DapperUserStore(DbConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		using var connection = _connectionFactory.CreateConnection();

		const string sql = @"
            INSERT INTO Usuario (
                Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
                PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
                TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Role,
                RefreshToken, RefreshTokenExpiryTime
            )
            VALUES (
                @Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed,
                @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed,
                @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @Role,
                @RefreshToken, @RefreshTokenExpiryTime
            )";

		var rowsAffected = await connection.ExecuteAsync(sql, user);
		return rowsAffected > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Não foi possível criar o utilizador na base de dados." });
	}

	public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		using var connection = _connectionFactory.CreateConnection();

		const string sql = @"
            UPDATE Usuario SET
                UserName = @UserName,
                NormalizedUserName = @NormalizedUserName,
                Email = @Email,
                NormalizedEmail = @NormalizedEmail,
                EmailConfirmed = @EmailConfirmed,
                PasswordHash = @PasswordHash,
                SecurityStamp = @SecurityStamp,
                ConcurrencyStamp = @ConcurrencyStamp,
                PhoneNumber = @PhoneNumber,
                PhoneNumberConfirmed = @PhoneNumberConfirmed,
                TwoFactorEnabled = @TwoFactorEnabled,
                LockoutEnd = @LockoutEnd,
                LockoutEnabled = @LockoutEnabled,
                AccessFailedCount = @AccessFailedCount,
                Role = @Role,
                RefreshToken = @RefreshToken,
                RefreshTokenExpiryTime = @RefreshTokenExpiryTime
            WHERE Id = @Id";

		var rowsAffected = await connection.ExecuteAsync(sql, user);
		return rowsAffected > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Não foi possível atualizar o utilizador. O registo pode não ter sido encontrado." });
	}

	public async Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		using var connection = _connectionFactory.CreateConnection();
		return await connection.QuerySingleOrDefaultAsync<Usuario>("SELECT * FROM Usuario WHERE Id = @Id", new { Id = userId });
	}

	public async Task<Usuario?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		using var connection = _connectionFactory.CreateConnection();
		return await connection.QuerySingleOrDefaultAsync<Usuario>("SELECT * FROM Usuario WHERE NormalizedEmail = @NormalizedEmail", new { NormalizedEmail = normalizedEmail });
	}

	public async Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
	{
		return await FindByEmailAsync(normalizedUserName, cancellationToken);
	}

	public Task<string?> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);
	public Task SetPasswordHashAsync(Usuario user, string? passwordHash, CancellationToken cancellationToken) { user.PasswordHash = passwordHash; return Task.CompletedTask; }
	public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash != null);

	public void Dispose() { }

	public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.Id);
	public Task<string?> GetUserNameAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
	public Task SetUserNameAsync(Usuario user, string? userName, CancellationToken cancellationToken) { user.UserName = userName; return Task.CompletedTask; }
	public Task<string?> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedUserName);
	public Task SetNormalizedUserNameAsync(Usuario user, string? normalizedName, CancellationToken cancellationToken) { user.NormalizedUserName = normalizedName; return Task.CompletedTask; }
	public Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken) => throw new NotImplementedException();
	public Task SetEmailAsync(Usuario user, string? email, CancellationToken cancellationToken) { user.Email = email; return Task.CompletedTask; }
	public Task<string?> GetEmailAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.Email);
	public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.EmailConfirmed);
	public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken) { user.EmailConfirmed = confirmed; return Task.CompletedTask; }
	public Task<string?> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedEmail);
	public Task SetNormalizedEmailAsync(Usuario user, string? normalizedEmail, CancellationToken cancellationToken) { user.NormalizedEmail = normalizedEmail; return Task.CompletedTask; }
}
