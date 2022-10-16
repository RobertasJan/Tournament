using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Shared.User
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Email, Login),
            new (ClaimTypes.Hash, Password),
        }, "Email"));
        public static UserModel FromClaimsPrincipal(ClaimsPrincipal principal) => new()
        {
            Login = principal.FindFirstValue(ClaimTypes.Email),
            Password = principal.FindFirstValue(ClaimTypes.Hash)
        };
    }
}
