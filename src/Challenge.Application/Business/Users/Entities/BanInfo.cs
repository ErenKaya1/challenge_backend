using System;

namespace Challenge.Application.Business.Users.Entities
{
    public class BanInfo
    {
        public bool IsBanned { get; set; }
        public string BanReason { get; set; }
        public DateTime? BanDate { get; set; }
    }
}