using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace web_server
{
    static class WebSha256
    {
        public static string CalcSha256(string uniqueKey)
        {
            string outSha256ToString = "";
            SHA256 sha256 = SHA256.Create();
            byte[] input = Encoding.UTF8.GetBytes(uniqueKey);
            byte[] output = sha256.ComputeHash(input);
            outSha256ToString = Convert.ToBase64String(output);
            return outSha256ToString;
        }
    }    
}