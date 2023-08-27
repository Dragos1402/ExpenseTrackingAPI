﻿namespace ExpenseTrackingAPI.Models
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SigningKey { get; set; }
    }

}
