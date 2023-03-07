using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WeixinCommon
{
    public class Security
    {
        public static string AESEncrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESDecrypt(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string WXAESDecrypt(string iv,string key,string inputdata)
        {
            try
            {
                String AesIV = iv.Replace(" ", "+");
                String AesKey = key.Replace(" ", "+");
                inputdata = inputdata.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(inputdata);

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(AesKey); // Encoding.UTF8.GetBytes(AesKey);
                rijndaelCipher.IV = Convert.FromBase64String(AesIV);// Encoding.UTF8.GetBytes(AesIV);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);

                return result;
            }
            catch (Exception)
            {
                return null;

            }
        }

        static void Main(string[] args)
        {
            String encryptData = Security.AESEncrypt("Test String", "1234567812345678", "1234567812345678");
            Console.WriteLine(encryptData);

            String decryptData = Security.AESDecrypt("2fbwW9+8vPId2/foafZq6Q==", "1234567812345678", "1234567812345678");
            Console.WriteLine(decryptData);

            Console.Read();
        }
    }
}