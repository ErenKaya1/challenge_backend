using System.ComponentModel.DataAnnotations;

namespace Challenge.Core.Enums
{
    public static class Enums
    {
        public enum RecordStatus
        {
            Deleted,
            Active
        }

        public enum UserRole
        {
            [Display(Name = "Admin")]
            Admin,

            [Display(Name = "Moderatör")]
            Moderator,

            [Display(Name = "Kullanıcı")]
            User
        }
    }
}