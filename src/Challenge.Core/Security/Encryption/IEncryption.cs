namespace Challenge.Core.Security.Encryption
{
    public interface IEncryption
    {
        string EncryptText(string value);
        string DecryptText(string value);
    }
}