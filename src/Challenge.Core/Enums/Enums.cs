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

        public enum GameMode : byte
        {
            [Display(Name = "Tek Oyunculu")]
            Single,

            [Display(Name = "İkili Oyun")]
            PublicDuo,

            [Display(Name = "Özel İkili Oyun")]
            PrivateDuo,

            [Display(Name = "Grup")]
            PublicGroup,

            [Display(Name = "Özel Grup")]
            PrivateGroup,
        }
    }
}