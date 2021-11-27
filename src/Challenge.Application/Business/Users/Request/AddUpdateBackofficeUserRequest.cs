using System;
using Challenge.Core.Attributes;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Request
{
    public class AddUpdateBackofficeUserRequest
    {
        public string Id { get; set; }

        [CustomRequired]
        public string FirstName { get; set; }

        [CustomRequired]
        public string LastName { get; set; }

        [CustomRequired]
        [EmailField]
        public string Email { get; set; }

        [CustomRequired]
        [PhoneNumberField]
        public string PhoneNumber { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public string RegisterIpAddress { get; set; }
        public UserRole Role { get; set; }
    }
}