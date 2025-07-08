namespace CludeMedSync.Models.Response
{
    public record TokenResponse(
        string AccessToken,
        string RefreshToken
    );
}