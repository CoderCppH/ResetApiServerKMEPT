using System.Security.Cryptography;
using System.Text;
namespace web_server
{
    class WebAse
    {
        private static char[] alphs = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890qwertyuiopasdfghjklzxcvbn".ToCharArray();
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                return null;
            if (Key == null || Key.Length <= 0)
               return null;
            if (IV == null || IV.Length <= 0)
                return null;
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            swEncrypt.Write(plainText);
                        encrypted = msEncrypt.ToArray();
                    }
                
            }

            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                return null;
            if (Key == null || Key.Length <= 0)
                return null;
            if (IV == null || IV.Length <= 0)
                return null;

            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
            }

            return plaintext;
        }

        public static byte[] GenerateKey(){
            string buffKey = "";
            for(int i = 0; i < 16; i++)
                buffKey += alphs[new Random().Next(0, alphs.Length-1)];
            return Encoding.UTF8.GetBytes(buffKey);
        }
         public static byte[] GenerateVecotr(){
            string buffKey = "";
            for(int i = 0; i < 16; i++)
                buffKey += alphs[new Random().Next(0, alphs.Length-1)];
            return Encoding.UTF8.GetBytes(buffKey);
        }

    }
}