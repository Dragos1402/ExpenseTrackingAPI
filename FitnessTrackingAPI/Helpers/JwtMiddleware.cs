using ExpenseTrackingAPI.Interfaces;
using ExpenseTrackingAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ExpenseTrackingAPI.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IAccountServ accountServ)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                AttachUserToContext(context, accountServ, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IAccountServ accountServ, string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSettings.SigningKey);

                // Configure token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateLifetime = true // Enable lifetime validation
                };

                // Validate the token and extract claims
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                var expirationClaim = claimsPrincipal.FindFirst("exp");
                if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long expirationUnixTime))
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expirationUnixTime).UtcDateTime;
                    var currentTime = DateTime.UtcNow;

                    if (expirationTime > currentTime)
                    {
                        // Token is not expired, you can update the token or perform any necessary actions here
                        // Example: accountServ.UpdateToken(token);
                    }
                    else
                    {
                        // Token is expired, handle it accordingly
                        // Example: accountServ.HandleExpiredToken(token);
                    }
                }
                else
                {
                    // Invalid token format or missing expiration claim, handle it accordingly
                    // Example: accountServ.HandleInvalidToken(token);
                }
            }
            catch (Exception ex)
            {
                // Handle token validation exceptions or logging as per your application's requirements
                throw new SecurityTokenException("Token validation failed.", ex);
            }
        }

    }
}
