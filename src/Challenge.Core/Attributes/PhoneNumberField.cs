using System.ComponentModel.DataAnnotations;
using Challenge.Core.Extensions;

namespace Challenge.Core.Attributes
{
    public class PhoneNumberField : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(this.ErrorMessage))
                this.ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz.";

            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && !value.ToString().IsValidPhoneNumber())
                return false;

            return true;
        }
    }
}