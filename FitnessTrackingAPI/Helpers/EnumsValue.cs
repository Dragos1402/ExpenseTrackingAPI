//using static ExpenseTrackingAPI.Models.DbModels.Transactions;

//namespace ExpenseTrackingAPI.Helpers
//{
//    public class EnumsValue
//    {
//        public string GetCategoryString(TransactionCategories category)
//        {
//            switch (category)
//            {
//                case TransactionCategories.Food:
//                    return "Food";
//                case TransactionCategories.Clothing:
//                    return "Clothing";
//                case TransactionCategories.Education:
//                    return "Education";
//                case TransactionCategories.Gift:
//                    return "Gift";
//                case TransactionCategories.Transport:
//                    return "Transport";
//                case TransactionCategories.Culture:
//                    return "Culture";
//                case TransactionCategories.Social:
//                    return "Social";
//                case TransactionCategories.Beauty:
//                    return "Beauty";
//                case TransactionCategories.House:
//                    return "House";
//                case TransactionCategories.Health:
//                    return "Health";
//                default:
//                    throw new ArgumentException("Invalid category value.");
//            }
//        }
//        public string GetTypeString(int type)
//        {
//            switch ((TransactionType)type)
//            {
//                case TransactionType.Card:
//                    return "Card";
//                case TransactionType.Bank_Transfer:
//                    return "Bank Transfer";
//                case TransactionType.Cash:
//                    return "Cash";
//                default:
//                    throw new ArgumentException("Invalid Transaction Value.");
//            }
//        }
//    }
//}
