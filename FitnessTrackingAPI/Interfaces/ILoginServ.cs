namespace ExpenseTrackingAPI.Interfaces
{
    public interface ILoginServ
    {
        void token_Login(HttpContext context, string token);
    }
}
