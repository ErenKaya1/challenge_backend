using System;
using Challenge.Common;

namespace Challenge.Application.Users.Entities
{
    public class User : AggregateRoot<string>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BirthDate { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; } = new();
        public PhoneNumberConfirmation PhoneNumberConfirmation { get; set; } = new();
    }

    public class EmailConfirmation
    {
        public bool IsVerified { get; set; }
        public string Code { get; set; }
        public DateTime? LastEmailSentDate { get; set; }
    }

    public class PhoneNumberConfirmation
    {
        public bool IsVerified { get; set; }
        public string Code { get; set; }
        public DateTime? LastCodeSendDate { get; set; }
    }
}