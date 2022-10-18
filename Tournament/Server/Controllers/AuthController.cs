using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tournament.Client.Services;
using Tournament.Domain.Services.User;
using Tournament.Domain.User;
using Tournament.Shared;

namespace Tournament.Server.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private string CreateJWT(ApplicationUserEntity user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppSettings.SecretKey));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.Name, user.Email));
            claimList.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claimList.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claimList.Add(new Claim(JwtRegisteredClaimNames.Jti, user.Email));
            //foreach (var role in user.AppUserRoles.Select(x => x.Role))
            //{
            //    claimList.Add(new Claim(ClaimTypes.Role, role.Role));
            //}

            var token = new JwtSecurityToken(issuer: AppSettings.ValidIssuer, audience: AppSettings.ValidAudience, claims: claimList, expires: DateTime.Now.AddDays(AppSettings.TokenLengthInDays), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IUserService userDb { get; set; }

        public AuthController(IUserService userDb)
        {
            this.userDb = userDb;
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<LoginResult> Post([FromBody] RegistrationModel reg)
        {
            if (string.IsNullOrWhiteSpace(reg.Email) || string.IsNullOrWhiteSpace(reg.Password))
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(reg.Email))
                    errorMessage += " Email address is empty;";
                if (string.IsNullOrWhiteSpace(reg.Password))
                    errorMessage += " Password is empty;";
                return new LoginResult { Message = errorMessage, Success = false };
            }

            if (reg.Password != reg.ConfirmPassword)
                return new LoginResult { Message = "Password and confirm password do not match.", Success = false };

            var user = await userDb.Create(new Domain.User.ApplicationUserEntity()
            {
                Email = reg.Email,
                EmailConfirmed = true,
                UserName = reg.Email
            }, reg.Password);
            if (user != null)
                return new LoginResult { Message = "Registration successful.", JwtBearer = CreateJWT(user), Email = reg.Email, Success = true };
            return new LoginResult { Message = "User already exists.", Success = false };
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<LoginResult> Post([FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(login.Email))
                    errorMessage += " Email address is empty;";
                if (string.IsNullOrWhiteSpace(login.Password))
                    errorMessage += " Password is empty;";
                return new LoginResult { Message = errorMessage, Success = false };
            }
            var user = await userDb.Login(login.Email, login.Password);

            if (user != null)
                return new LoginResult { Message = "Login successful.", JwtBearer = CreateJWT(user), Email = login.Email, Success = true };

            return new LoginResult { Message = "User/password not found.", Success = false };
        }
    }

}
