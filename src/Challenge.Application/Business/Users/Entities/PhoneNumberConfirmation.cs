using System;

namespace Challenge.Application.Business.Users.Entities
{
    public class PhoneNumberConfirmation
    {
        public bool IsVerified { get; set; }
        public string Code { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? LastCodeSendDate { get; set; }
    }
}