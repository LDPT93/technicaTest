using System.Security.Cryptography;
using System.Text;

namespace EncrypterDateProducer
{
    public class ClassSHA268generator
    {
        public static readonly SHA256 Sha256 = SHA256.Create();

        public string CurrentTimeToSha256()
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            byte[] bytes = Encoding.UTF8.GetBytes(currentTime);
            byte[] hashBytes = Sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
