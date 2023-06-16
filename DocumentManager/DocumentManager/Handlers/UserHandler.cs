using Azure.Core;
using DocumentManager.Data;
using DocumentManager.Models;
using DocumentManager.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DocumentManager.Handlers
{
    public class UserHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserServices _userServices;
        public UserHandler(IConfiguration configuration, UserServices userServices)
        {
            _configuration = configuration;
            _userServices = userServices;
        }
        public bool CreateUser(Account request)
        {
            User user = UserMapper(request);
            CreatePasswordHash(user, request.Password);

            return _userServices.InsertUser(user) > 0;
        }
        public string VerifyUser(Account request)
        {
           UserData userData = _userServices.SelectUser(request.Username);
           if (userData == null) { return ""; }
           if(VerifyPasswordHash(userData.User, request.Password))
            {
                return CreateToken(userData);
            }

            return "";
        }
        private User UserMapper(Account account)
        {
            return new User
            {
                Username = account.Username,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber
            };
        }
        private void CreatePasswordHash(User user, string password)
        {
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(User user, string password)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(user.PasswordHash);
            }
        }
        private string CreateToken(UserData userData)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userData.User.Username),
                new Claim(ClaimTypes.Role, userData.Role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
