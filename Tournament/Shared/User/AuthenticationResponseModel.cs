namespace Tournament.Shared.User
{
    public class AuthenticationResponseModel
    {
        public AuthenticationResponseModel(string accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public string? RefreshToken { get; }
    }
}
