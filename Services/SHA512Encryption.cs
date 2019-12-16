using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Services
{
    /// <summary>
    /// Algorytm haszujący SHA512 - wykorzystany podczas zapisywania tokenów w bazie danych
    /// </summary>
    public class SHA512Encryption
    {
        public string Hash(string data)
        {
            using (SHA512 sha = new SHA512Managed())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));

                return Encoding.UTF8.GetString(hashBytes);
            }
        }
    }
}
