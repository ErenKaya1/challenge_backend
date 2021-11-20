namespace Challenge.Core.Security.Hash
{
    public interface IHasher
    {
        string CreateHash(string value);
    }
}