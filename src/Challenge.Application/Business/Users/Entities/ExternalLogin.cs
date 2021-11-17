using System.Text.Json.Serialization;

namespace Challenge.Application.Business.Users.Entities
{
    public class ExternalLogin
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string ProviderId { get; set; }
        
        public string ProviderName { get; set; }
    }
}