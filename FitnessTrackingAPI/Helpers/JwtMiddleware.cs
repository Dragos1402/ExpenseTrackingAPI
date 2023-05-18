using ExpenseTrackingAPI.Interfaces;

namespace ExpenseTrackingAPI.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Ivoke(HttpContext context, ILoginServ loginServ)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                attachUserToContext(context, loginServ, token);
                await _next(context);
            }
        }
        private void attachUserToContext(HttpContext context, ILoginServ loginServ, string token)
        {
            try
            {
                loginServ.token_Login(context, token);
            }
            catch
            {

                throw;
            }
        }
    }
}
