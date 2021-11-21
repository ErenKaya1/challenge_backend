using System.ComponentModel.DataAnnotations;
using Challenge.Core.Extensions;

namespace Challenge.Core.Attributes
{
    public class EmailField : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(this.ErrorMessage))
                this.ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.";

            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && !value.ToString().IsValidEmailAddress())
                return false;

            return true;
        }
    }
}