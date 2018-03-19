using System.ComponentModel.DataAnnotations;

namespace MeetMusicModels.Models
{
    public class UserMusicFamily
    {
        [Required]
        [StringLength(36)]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        [StringLength(36)]
        public string FamilyId { get; set; }

        public MusicFamily MusicFamily { get; set; }

        [Required]
        public int Rank { get; set; }
    }
}
