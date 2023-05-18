namespace ExpenseTrackingAPI.Helpers
{
    public class ErrorMessage
    {
        public int msg_code { get; set; } = 0;
        public string msg_method { get; set; } = string.Empty;
        public string msg_techdata { get; set; } = string.Empty;
        public string msg_text { get; set; } = string.Empty;
        public string msg_type { get; set; } = string.Empty;
    }
}
