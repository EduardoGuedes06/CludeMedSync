namespace CludeMedSync.Models.Response
{
    public record TokenResponse(
        string AccessToken,
		DateTime AccessTokenExpiration,
		string RefreshToken
    );
}