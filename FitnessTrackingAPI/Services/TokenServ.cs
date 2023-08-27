using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Interfaces;
using ExpenseTrackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;

namespace ExpenseTrackingAPI.Services;

public class TokenServ
{
    private readonly IConfiguration _configuration;
    private readonly ExpenseContext _context;

    public DateTime TokenExpirationDate { get; set; }

    public TokenServ(IConfiguration configuration, ExpenseContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    public string GenerateToken(AccountLogin account)
    {
        if (account != null && !string.IsNullOrEmpty(account.email) && !string.IsNullOrEmpty(account.password))
        {
            AccountToken user = GetUser(account.email);
            this.TokenExpirationDate = DateTime.UtcNow.AddDays(1);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("user_id", user.user_id.ToString()),
                    new Claim("first_name", user.first_name.ToString()),
                    new Claim("last_name",user.last_name.ToString()),
                    new Claim("email",user.email.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SigningKey"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["JwtSettings:ValidIssuer"],
                    _configuration["JwtSettings:ValidAudience"],
                    claims,
                    expires: this.TokenExpirationDate,
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                throw new Exception("Invalid credentials");
            }
        }
        else
        {
            throw new ArgumentNullException("userInfo", "User information is null or incomplete");
        }
    }
    public AccountToken GetUser(string email)
    {

        var account =  (from d in _context.Accounts
                             where d.email == email
                             select new AccountToken
                             {
                                 user_id = d.user_id,
                                 email = d.email,
                                 first_name = d.first_name,
                                 last_name = d.last_name
                             }).FirstOrDefault();
        return account;
    }
}
