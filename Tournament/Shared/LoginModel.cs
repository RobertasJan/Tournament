using System.ComponentModel.DataAnnotations;

namespace Tournament.Shared;
public class LoginModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}
