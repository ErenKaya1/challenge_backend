using System;

namespace Challenge.Application.Business.Users.Entities
{
    public class EmailConfirmation
    {
        public bool IsVerified { get; set; }
        public string Code { get; set; }
        public DateTime? LastEmailSentDate { get; set; }
    }
}