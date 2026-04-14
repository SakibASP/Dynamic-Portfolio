using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Application.Common
{
    public class EncryptionHelper
    {
        //Encryption Key
        private static readonly string Key = "#SakibRealMadrid"; // Must be 16, 24, or 32 characters long

        // Encrypt a string
        public static string Encrypt(string? plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = new byte[16]; // Initialization vector, all zero by default

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        // Decrypt a string
        public static string Decrypt(string? encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = new byte[16]; // Must match the IV used in encryption

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encryptedText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);
            return reader.ReadToEnd();
        }
    }
}
