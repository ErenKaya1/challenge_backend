namespace Challenge.Application.Business.Users.Response
{
    public class GoogleTokenValidationResponse
    {
        public string email { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string sub { get; set; }
        public string picture { get; set; }
    }
}