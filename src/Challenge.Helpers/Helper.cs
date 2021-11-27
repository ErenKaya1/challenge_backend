using System;
using System.Linq;

namespace Challenge.Helpers
{
    public static class Helper
    {
        public static string GenerateKey(int length, bool onlyNumbers = false)
        {
            var chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpRrSsTtUuVvYyZz0123456789";

            if (onlyNumbers)
                chars = "0123456789";

            var random = new Random();
            var generatedPassword = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return generatedPassword;
        }
    }
}