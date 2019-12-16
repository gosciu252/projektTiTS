using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AspNetIdentitySample.Services
{
    /// <summary>
    /// Klasa służąca do wygenerowania ciągu znaków o dowolnej długości
    /// </summary>
    public class TokenGenerator
    {
        public string GenerateToken(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[256];

                rng.GetBytes(bytes);

                var str = Convert.ToBase64String(bytes);

                var rgx = new Regex("[^a-zA-Z0-9]");
                str = rgx.Replace(str, "");

                return str.Substring(0, length);
            }
        }
    }
}
