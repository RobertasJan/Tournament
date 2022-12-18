using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tournament.Domain;
using Tournament.Domain.Players;
using Tournament.Domain.Players.Exceptions;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Services.User;
using Tournament.Domain.User;
using Tournament.Shared;
using Tournament.Shared.Players;

namespace Tournament.Server.Controllers
{
    [ApiController]
    public class AuthController : BaseController
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
            if (user.IsAdmin)
            {
                claimList.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            //  claimList.Add(new Claim(JwtRegisteredClaimNames.Gender, user.Player.Gender.ToString()));
            //foreach (var role in user.AppUserRoles.Select(x => x.Role))
            //{
            //    claimList.Add(new Claim(ClaimTypes.Role, role.Role));
            //}

            var token = new JwtSecurityToken(issuer: AppSettings.ValidIssuer, audience: AppSettings.ValidAudience, claims: claimList, expires: DateTime.Now.AddDays(AppSettings.TokenLengthInDays), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IUserService userDb { get; set; }
        private IPlayerService playerService { get; set; }

        public AuthController(IUserService userDb, IPlayerService playerService)
        {
            this.userDb = userDb;
            this.playerService = playerService;
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<ResponseModel<LoginResult>> Register([FromBody] RegistrationModel reg)
        {
            try
            {
                ValidateRegistrationModel(reg);

                var user = await userDb.Create(new Domain.User.ApplicationUserEntity()
                {
                    Email = reg.Email,
                    EmailConfirmed = true,
                    UserName = reg.Email
                }, reg.Password);
                var playerId = await playerService.Create(new PlayerEntity()
                {
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    UserId = user.Id,
                    BirthDate = reg.BirthDate.Value,
                    Gender = reg.Gender
                }, CancellationToken.None);
                var player = await playerService.GetById(playerId, CancellationToken.None);
                if (user != null)
                    return new ResponseModel<LoginResult>(new LoginResult { Message = "Registration successful.", JwtBearer = CreateJWT(user), Email = reg.Email, Player = Mapper.Map<PlayerModel>(player), Success = true });

            }
            catch (APIException ex)
            {
                return new ResponseModel<LoginResult>(ex.ErrorCodeModel);
            }
            return new ResponseModel<LoginResult>(new LoginResult { Message = "User already exists.", Success = false });
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<ResponseModel<LoginResult>> Login([FromBody] LoginModel login)
        {
            try
            {
                ValidateLoginModel(login);
                var user = await userDb.Login(login.Email, login.Password);
                if (user is null)
                {
                    throw new InvalidPasswordException();
                }
                var player = await playerService.GetByUserId(user.Id, CancellationToken.None);

                var role = "";
                if (user.IsAdmin)
                {
                    role = "Admin";
                }
                return new ResponseModel<LoginResult>(new LoginResult { Message = "Login successful.", JwtBearer = CreateJWT(user), Email = login.Email, Player = Mapper.Map<PlayerModel>(player), Role = role, Success = true });
            }
            catch (APIException ex)
            {
                return new ResponseModel<LoginResult>(ex.ErrorCodeModel);
            }

        }

        private void ValidateRegistrationModel(RegistrationModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new NoEmailException();
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new NoPasswordException();
            }

            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                throw new NoConfirmPasswordException();
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                throw new NoFirstnameException();
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                throw new NoLastnameException();
            }

            if (!model.BirthDate.HasValue)
            {
                throw new NoBirthdateException();
            }

            if (model.BirthDate.Value.Date > DateTime.Today)
            {
                throw new BirthDateInTheFutureException();
            }


            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordsDoNotMatchException();
            }
        }
        private void ValidateLoginModel(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new NoEmailException();
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new NoPasswordException();
            }
        }
    }

}
