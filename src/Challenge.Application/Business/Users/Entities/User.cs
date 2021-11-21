using System.Collections.Generic;
using Challenge.Common;
using Challenge.Core.Attributes;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Entities
{
    public class User : AggregateRoot<string>
    {
        [CustomRequired]
        public string FirstName { get; set; }

        [CustomRequired]
        public string LastName { get; set; }

        [CustomRequired]
        [EmailField]
        public string Email { get; set; }

        [PhoneNumberField]
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