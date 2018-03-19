using System.ComponentModel.DataAnnotations;

namespace MeetMusicModels.Models
{
    public class MusicGenre : ModelBase
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]{3,40}$",
        ErrorMessage = "Music family name must be at least 3 characters and not exceed 40 characters")]
        public string Name { get; set; }
        
        [StringLength(36)]
        public string FamilyId { get; set; }

        public MusicFamily Family { get; set; }
    }
}
