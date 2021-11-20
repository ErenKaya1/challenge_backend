namespace Challenge.Core.Security.Hash
{
    public class Hasher : IHasher
    {
        public string Salt { get; set; }

        public string CreateHash(string value)
        {
            return value;
        }
    }
}