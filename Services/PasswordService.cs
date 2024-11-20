using System.Security.Cryptography;
using System.Text;

namespace PDAB.Services;

public class PasswordService 
{
    public byte[] HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}