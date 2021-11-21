using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Challenge.Core.Attributes
{
    public class MongoIdField : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(this.ErrorMessage))
                this.ErrorMessage = "Geçersiz parametre.";

            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                try
                {
                    new ObjectId(value.ToString());
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}