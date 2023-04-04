using System.Security.Cryptography;
using System.Text;

namespace WebQuiz.Models
{
    public class PasswordHasher
    {
        public static string HashPassword(string passToHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passToHash));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
