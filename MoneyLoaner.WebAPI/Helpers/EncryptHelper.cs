using System.Security.Cryptography;
using System.Text;

namespace MoneyLoaner.WebAPI.Helpers
{
    public static class EncryptHelper
    {
        private const string _KEY = ".p4qXiB*6kR6#CPP";
        private const string _IV = "L@kvdC,BX2tm?*P0";

        public static string Encrypt(string plainText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_KEY);
            aesAlg.IV = Encoding.UTF8.GetBytes(_IV);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_KEY);
            aesAlg.IV = Encoding.UTF8.GetBytes(_IV);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
    }
}
