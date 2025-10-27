using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BudgetP
{
    public class TMPLTCrypto
    {
        private static string _encyptionKeyValue;

        static TMPLTCrypto()
        {
            TMPLTCrypto._encyptionKeyValue = "SimplifiedadminBusinessApplication";
        }

        public TMPLTCrypto()
        {
        }

        public static string Decrypt(string cipherText)
        {
            return TMPLTCrypto.Decrypt(cipherText, TMPLTCrypto._encyptionKeyValue, "sa");
        }

        public static string Decrypt(string cipherText, string salt)
        {
            string str = TMPLTCrypto.Decrypt(cipherText, TMPLTCrypto._encyptionKeyValue, salt.ToLower());
            return str;
        }

        public static string Decrypt(string cipherText, string Password, string salt)
        {
            byte[] numArray = Convert.FromBase64String(cipherText);
            byte[] bytes = Encoding.Unicode.GetBytes(salt);
            PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(Password, bytes);
            byte[] numArray1 = TMPLTCrypto.Decrypt(numArray, passwordDeriveByte.GetBytes(32), passwordDeriveByte.GetBytes(16));
            return Encoding.Unicode.GetString(numArray1);
        }

        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            byte[] bytes;
            MemoryStream memoryStream = new MemoryStream();
            Rijndael key = Rijndael.Create();
            key.Key = Key;
            key.IV = IV;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, key.CreateDecryptor(), CryptoStreamMode.Write);
            if (cipherData.Length == 0)
            {
                bytes = Encoding.Unicode.GetBytes("");
            }
            else
            {
                cryptoStream.Write(cipherData, 0, (int)cipherData.Length);
                cryptoStream.Close();
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        public static string Encrypt(string clearData)
        {
            return TMPLTCrypto.Encrypt(clearData, TMPLTCrypto._encyptionKeyValue, "sa");
        }

        public static string Encrypt(string clearData, string salt)
        {
            string str = TMPLTCrypto.Encrypt(clearData, TMPLTCrypto._encyptionKeyValue, salt.ToLower());
            return str;
        }

        public static string Encrypt(string clearText, string Password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(clearText);
            byte[] numArray = Encoding.Unicode.GetBytes(salt);
            PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(Password, numArray);
            byte[] numArray1 = TMPLTCrypto.Encrypt(bytes, passwordDeriveByte.GetBytes(32), passwordDeriveByte.GetBytes(16));
            return Convert.ToBase64String(numArray1);
        }

        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream memoryStream = new MemoryStream();
            Rijndael key = Rijndael.Create();
            key.Key = Key;
            key.IV = IV;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, key.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(clearData, 0, (int)clearData.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }
    }
}