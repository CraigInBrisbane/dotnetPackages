using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Providers.Encryption
{
    public static class EncryptionProvider
    {
        public static string GenerateSaltedHash(string plainText, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var passwordBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Encoding.UTF8.GetBytes(salt);


            var plainTextWithSaltBytes =
                new byte[((ICollection) passwordBytes).Count + ((ICollection) saltBytes).Count];

            for (var i = 0; i < ((ICollection) passwordBytes).Count; i++)
            {
                plainTextWithSaltBytes[i] = passwordBytes[i];
            }

            for (var i = 0; i < ((ICollection) saltBytes).Count; i++)
            {
                plainTextWithSaltBytes[((ICollection) passwordBytes).Count + i] = saltBytes[i];
            }

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
    }
}