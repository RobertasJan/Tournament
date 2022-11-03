using Tournament.Shared.Players;

namespace Tournament.Shared;
public class LoginResult
{
    public string Message { get; set; }
    public string Email { get; set; }
    public string JwtBearer { get; set; }
    public string Role { get; set; }
    public bool Success { get; set; }
    public PlayerModel Player { get; set; }
}
