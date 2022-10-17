using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.User;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.User
{
    public interface IUserService
    {
        public Task<ApplicationUserEntity> Create(ApplicationUserEntity user, string password);
        public Task<ApplicationUserEntity?> Login(string loginName, string password);
        public Task Signout();
        public Task AddClaim(ApplicationUserEntity user, string type, string value);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;

        private static string CreateHash(string password)
        {
            var salt = "997eff51db1544c7a3c2ddeb2053f052";
            var md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt + password));
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(data);
        }

        public UserService(AppDbContext db, UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager)
        {
            this._db = db;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<ApplicationUserEntity> Create(ApplicationUserEntity user, string password)
        {
            user.LockoutEnabled = false;
            await _userManager.CreateAsync(user, password);
            return user;
        }

        public async Task<ApplicationUserEntity?> Login(string loginName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(loginName, password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginName);
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Email, loginName));
                return user;
            }
            return null;
        }

        public async Task AddClaim(ApplicationUserEntity user, string type, string value)
        {
            await _userManager.AddClaimAsync(user, new Claim(type, value));
        }

        public async Task Signout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
