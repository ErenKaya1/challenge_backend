using System;
using System.Text;

namespace Challenge.Core.Security.Encryption
{
    public class Encryption : IEncryption
    {
        public string PrivateKey { get; set; }

        public string EncryptText(string value)
        {
            return value;
        }

        public string DecryptText(string value)
        {
            return value;
        }

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(data);
        }
    }
}