using System.Security.Cryptography;
using System.Text;

namespace RestoGestApp.Helpers;

public static class PasswordHelper
{
    // Simple password hashing using SHA256
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            
            return builder.ToString();
        }
    }
    
    // Verify password against stored hash
    public static bool VerifyPassword(string password, string storedHash)
    {
        string hashedPassword = HashPassword(password);
        return hashedPassword.Equals(storedHash);
    }
}
