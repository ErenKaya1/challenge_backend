using System.ComponentModel.DataAnnotations;

namespace Challenge.Core.Enums
{
    public static class Enums
    {
        public enum RecordStatus : byte
        {
            Deleted,
            Active
        }

        public enum UserRole : byte
        {
            [Display(Name = "Admin")]
            Admin,

            [Display(Name = "Moderatör")]
            Moderator,

            [Display(Name = "Kullanıcı")]
            User
        }

        public enum PolicyType : byte
        {
            [Display(Name = "KVKK")]
            KVKK,
        }
    }
}