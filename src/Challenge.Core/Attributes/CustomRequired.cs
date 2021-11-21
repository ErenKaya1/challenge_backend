using System.ComponentModel.DataAnnotations;

namespace Challenge.Core.Attributes
{
    public class CustomRequired : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(this.ErrorMessage))
                this.ErrorMessage = "Lütfen bu alanı doldurun.";

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || (value != null && value.ToString() == "1.01.0001 00:00:00"))
                return false;

            return true;
        }
    }
}