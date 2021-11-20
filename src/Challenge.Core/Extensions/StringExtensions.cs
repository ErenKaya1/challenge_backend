using System.Globalization;
using System.Linq;

namespace Challenge.Core.Extensions
{
    public static class StringExtensions
    {
        public static (string, string) GetFirstAndLastNameByFullName(this string fullName)
        {
            var firstName = "";
            var lastName = "";
            var splitted = fullName.Trim().Split(' ').ToList();

            if (splitted.Count == 1)
                firstName = splitted[0].Trim().ToLower().FirstCharToUpper();
            else
            {
                lastName = splitted.Last().Trim().ToLower().FirstCharToUpper();
                splitted.RemoveAt(splitted.Count - 1);

                foreach (var word in splitted)
                    firstName = firstName + " " + word.Trim().ToLower().FirstCharToUpper();
            }

            return (firstName, lastName);
        }

        public static string FirstCharToUpper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.First().ToString().ToUpper(new CultureInfo("tr-TR")) + value.Substring(1);
        }
    }
}