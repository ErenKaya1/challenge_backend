using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

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

        public static bool IsValidEmailAddress(this string value)
        {
            try
            {
                var temp = new MailAddress(value);

                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(value))
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsValidPhoneNumber(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false;

                string validPattern = @"\+[0-9]{4,14}$";
                var regex = new Regex(validPattern, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(value))
                    return false;
            }
            catch
            {
                return false;
            }
            
            return true;
        }
    }
}