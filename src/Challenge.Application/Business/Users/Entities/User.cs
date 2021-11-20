using System;
using System.Collections.Generic;
using Challenge.Common;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Entities
{
    public class User : AggregateRoot<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string SignUpIpAddress { get; set; }
        public string Password { get; set; }
        public bool SetupCompleted { get; set; }
        public bool HideMyData { get; set; }
        public UserRole Role { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; } = new();
        public PhoneNumberConfirmation PhoneNumberConfirmation { get; set; } = new();
        public List<ExternalLogin> ExternalLogins { get; set; } = new();
        public BanInfo BanInfo { get; set; } = new();
    }
}