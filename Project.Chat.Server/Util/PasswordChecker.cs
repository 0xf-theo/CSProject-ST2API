using DevOne.Security.Cryptography.BCrypt;

namespace Project.Chat.Server.Util
{
    public class PasswordChecker
    {

        public static bool CheckPassword(string bcryptPassword, string password)
        {
            return BCryptHelper.CheckPassword(password, bcryptPassword);
        }

        public static string HashPassword(string password)
        {
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(12));
        }
    }
}
