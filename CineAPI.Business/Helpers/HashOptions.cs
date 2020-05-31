using System;
using System.Security.Cryptography;
using System.Text;

namespace CineAPI.Business.Helpers
{
    public class HashOptions
    {
        public static string CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                string salt = Convert.ToBase64String(passwordSalt);
                string hash = Convert.ToBase64String(passwordHash);

                return $"{salt}.{hash}";
            }
        }

        public static bool VerifyPasswordHash(string password, string passwordHash)
        {
            var hashs = passwordHash.Split('.');

            var storedSalt = Convert.FromBase64String(hashs[0]);
            var storedHash = Convert.FromBase64String(hashs[1]);

            using (var hmac = new HMACSHA256(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }

                return true;
            }
        }
    }
}