using Microsoft.AspNetCore.Identity;
using Tournament.Domain.Players;

namespace Tournament.Domain.User
{
    public class ApplicationUserEntity : IdentityUser
    {
        public PlayerEntity Player { get; set; }
    }
}
