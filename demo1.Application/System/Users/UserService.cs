using BHShopSolution.Utilities.Exceptions;
using demo1.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopBHSolution.ViewModels.System;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Application.System.Users
{
    public class UserService : IUserService
    {
        //thuvien Microsoft.AspNetCore.Identity;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;


        public UserService(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task<string> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null ;
            var result = await _signInManager.PasswordSignInAsync(user,request.Password,request.RememberMe,true);
            var roles = await _userManager.GetRolesAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }
            var claims = new[] { new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.GivenName, user.FirstName), new Claim(ClaimTypes.Name, request.UserName), new Claim(ClaimTypes.Role, String.Join(";", roles)) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return false;
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return false;
            }

            user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
