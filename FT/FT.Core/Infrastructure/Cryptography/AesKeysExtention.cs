using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FT.Core.Infrastructure.Cryptography
{
    public static class AesExtention
    {
        public static string Encrypt(this string plainText, string key)
        {
            try
            {

                byte[] array;

                using (Aes aes = Aes.Create())

                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    byte[] iv = new byte[16];
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new MemoryStream();
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }

                return Convert.ToBase64String(array);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static string Decrypt(this string cipherText, string key)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                byte[] iv = new byte[16];
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] buffer = Convert.FromBase64String(cipherText);
                using MemoryStream memoryStream = new MemoryStream(buffer);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
