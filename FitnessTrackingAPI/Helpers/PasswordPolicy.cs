using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ExpenseTrackingAPI.Helpers
{
    public class PasswordPolicy
    {
        public enum Password_Policy
        {
            [Description("1")] Min8_Alfa1_Number1,
            [Description("2")] Min8_AlfaUpper1_Number1,
            [Description("3")] Min8_Alfa1_Number1_Special,
            [Description("4")] Min8_AlfaUpper1_Number1_Special,
        }
        public static bool PasswordCheck(string password, Password_Policy type)
        {
            bool functionReturnValue=false;
            string pattern = "";
            switch (type)
            {
                case Password_Policy.Min8_AlfaUpper1_Number1:
                    pattern = "^(?=.*[A-Za-z])(?=.*\\d[A-Za-z\\d]{8,}$";
                    break;
                case Password_Policy.Min8_Alfa1_Number1:
                    pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$";
                    break;
                case Password_Policy.Min8_AlfaUpper1_Number1_Special:
                    pattern = "^(?=.*[a-z])(?=.(\\d)(?=.*[$_@$!%*#?&])[A-Za-z\\d$_@$!%*#?&]{8,}$";
                    break;
                case Password_Policy.Min8_Alfa1_Number1_Special:
                    pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$_@$!%*?&])[A-Za-z\\d$_@$!%*?&]{8,}";
                    break;
                default:
                    pattern = "^(?=.*[a-z])(?=.*[A-Z])";
                    break;
            }
            Match RegPasswordCheck = Regex.Match(password, pattern);
            if (RegPasswordCheck.Success)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            return functionReturnValue;
        }
    }
}
