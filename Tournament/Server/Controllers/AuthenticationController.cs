using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.User;
using Tournament.Shared.User;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserService userService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task Register(UserModel model, CancellationToken cancellationToken)
        {
            await userService.Create(new Domain.User.ApplicationUserEntity()
            {
                Email = model.Login,
                EmailConfirmed = true,
                UserName = model.Login
            }, model.Password);
            await Login(model, cancellationToken);
        }

        [HttpPost("login")]
        public async Task Login(UserModel model, CancellationToken cancellationToken)
        {
            var response = await userService.Login(model.Login, model.Password);
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
