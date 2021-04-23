using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateSignature();
            Console.ReadKey(true);
        }

        public static void GenerateSignature()
        {
            var version = "1";
            var key = "12345678";
            var nonce = System.Guid.NewGuid().ToString("N");
            var timestamp = GetTimestamp(DateTime.Now);

            var head = $"{version}-{key}-{nonce}-{timestamp}";

            var message = new StringBuilder();
            var param = new Dictionary<string, string> { { "stationId", "0CF70375-A8F0-40E8-B834-4FD57CD60E18" } };
            if (param != null && param.Any())
            {
                foreach (var p in param)
                {
                    if (message.Length > 0)
                    {
                        message.Append("&");
                    }
                    message.Append(p.Key);
                    message.Append("=");
                    message.Append(p.Value);
                }
            }

            var combieMessage = $"{head}\r\n{message.ToString()}";
            combieMessage = @"1-12345678-6810cf5e5c1e4e22a54a6f969192d0c4-1619143875
stationId=0CF70375-A8F0-40E8-B834-4FD57CD60E18";
            var temp = HmacSHA256(combieMessage, "11111111111111111111", Encoding.ASCII);
            byte[] bytes = Encoding.UTF8.GetBytes(temp);

            Console.WriteLine(temp);
            Console.WriteLine(Convert.ToBase64String(bytes));

            Console.WriteLine("\n");

            Console.WriteLine(temp == "692d974639d7717ff087bac40c6804b484415d6498b5c0c4766bd40f29bc966c");
            Console.WriteLine(temp == "aS2XRjnXcX/wh7rEDGgEtIRBXWSYtcDEdmvUDym8lmw=");
            Console.WriteLine(Convert.ToBase64String(bytes) == "aS2XRjnXcX/wh7rEDGgEtIRBXWSYtcDEdmvUDym8lmw=");

            // HmacSHA256     692d974639d7717ff087bac40c6804b484415d6498b5c0c4766bd40f29bc966c
            // ToBase64String aS2XRjnXcX/wh7rEDGgEtIRBXWSYtcDEdmvUDym8lmw=
        }

        public static long GetTimestamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000000;   //除10000000调整为10位      
            return t;
        }

        public static string HmacSHA256(string message, string secret, Encoding encoding = null)
        {
            secret = secret ?? "";
            if (encoding == null)
            {
                encoding = new System.Text.UTF8Encoding();
            }
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
