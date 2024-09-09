using Shared.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EncrypterDateProducer.Service
{
    public static class SHA268generator
    {
        public static readonly SHA256 Sha256 = SHA256.Create();

        public static string CurrentTimeToSha256()
        {
            DateTime utcNow = DateTime.UtcNow;
            string currentTime = utcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            byte[] currentTimeByteParsed = Encoding.UTF8.GetBytes(currentTime);
            byte[] currentTimeHashed = Sha256.ComputeHash(currentTimeByteParsed);
            string currentTimeHashedResult = BitConverter.ToString(currentTimeHashed).Replace("-", "").ToLower();
            var message = new MessageDTO { OriginDate = utcNow, HashedDate = currentTimeHashedResult };
            return JsonSerializer.Serialize(message);
        }
    }
}
