namespace ExpenseTrackingAPI.Helpers
{
    public class Response
    {
        public Boolean success { get; set; } = true;
        public ErrorMessage error_message { get; set; }
        public object data { get; set; }
        public Boolean result { get; set; } = true;

        //public int result_key { get; set; } = 0;
        //public string token_value { get; set; } = string.Empty;
        public Response()
        {
            error_message = new ErrorMessage();
        }
        public void SetResponse(string code, string Method, Exception? ex =null)
        {
            switch (code)
            {
                case ErrorCodes.SUCCESS:
                    success = true;
                    result = true;
                    error_message.msg_code = Convert.ToInt32(code);
                    error_message.msg_method= Method;
                    error_message.msg_techdata = "Success";
                    error_message.msg_text = "Success";
                    error_message.msg_type = "Success";
                    break;
                case ErrorCodes.DATABASE_READING_ERROR:

                    result = false;
                    success = false;
                    error_message.msg_code = Convert.ToInt32(code);
                    error_message.msg_method = Method;
                    error_message.msg_techdata = "There was an error in the Database while reading the data (Data missing or an error occured)";
                    error_message.msg_text= "There was an error in the Database while reading the data (Data missing or an error occured)";
                    error_message.msg_type= "There was an error in the Database while reading the data (Data missing or an error occured)";
                    break;
                case ErrorCodes.DATABASE_WRITING_ERROR:

                    result = false;
                    success = false;
                    error_message.msg_code = Convert.ToInt32(code);
                    error_message.msg_method = Method;
                    error_message.msg_techdata = "There was an error while writing the data (Data bad format or an error orccured)";
                    error_message.msg_text = "There was an error while writing the data (Data bad format or an error orccured)";
                    error_message.msg_type = "There was an error while writing the data (Data bad format or an error orccured)";
                    break;
                case ErrorCodes.UNAUTHORIZED:
                    result = false;
                    success = false;
                    error_message.msg_code = Convert.ToInt32(code);
                    error_message.msg_method = Method;
                    error_message.msg_techdata = "User Unauthorized (Missing authorization data)";
                    error_message.msg_text = "User Unauthorized (Missing authorization data)";
                    error_message.msg_type = "User Unauthorized (Missing authorization data)";
                    break;
                case ErrorCodes.ACCOUNT_NOT_FOUND:
                    result = false;
                    success = false;
                    error_message.msg_code= Convert.ToInt32(code);
                    error_message.msg_method = Method;
                    error_message.msg_techdata = "Account Not Found (Bad Credentials)";
                    error_message.msg_text= "Account Not Found (Bad Credentials)";
                    error_message.msg_type = "Account Not Found (Bad Credentials)";
                    break;
            }
        }
    }
}
