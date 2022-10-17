using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Tournament.Domain.Players;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Services.User;
using Tournament.Shared.User;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserService userService;
        private readonly IPlayerService playerService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService, IPlayerService playerService)
        {
            _logger = logger;
            this.userService = userService;
            this.playerService = playerService;
        }

        [HttpPost("register")]
        public async Task Register(UserModel model, CancellationToken cancellationToken)
        {
            var user = await userService.Create(new Domain.User.ApplicationUserEntity()
            {
                Email = model.Login,
                EmailConfirmed = true,
                UserName = model.Login
            }, model.Password);
            var guid = await playerService.Create(new PlayerEntity()
            {
                FirstName = model.Login,
                LastName = model.Login,
                UserId = user.Id
            }, cancellationToken);
            await userService.AddClaim(user, "PlayerId", guid.ToString());
            await Login(model, cancellationToken);
        }

        [HttpPost("login")]
        public async Task Login(UserModel model, CancellationToken cancellationToken)
        {
            var response = await userService.Login(model.Login, model.Password);
            var guid = await playerService.GetByUserId(response.Id, cancellationToken);
            await userService.AddClaim(response, "PlayerId", guid.Id.ToString());
        }

        [HttpPost("signout")]
        public async Task SignOut(CancellationToken cancellationToken)
        {
            await userService.Signout();
        }

        private static string CreateHash(string password)
        {
            var salt = "997eff51db1544c7a3c2ddeb2053f052";
            var md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt + password));
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(data);
        }
    }
}
