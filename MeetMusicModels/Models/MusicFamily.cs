using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace MeetMusicModels.Models
{
    public class MusicFamily : ModelBase
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]{3,40}$",
        ErrorMessage = "Music family name must be at least 3 characters and not exceed 40 characters")]
        public string Name { get; set; }

        public List<MusicGenre> Genres { get; set; }

        public ICollection<UserMusicFamily> UserMusicFamilies { get; set; }
    }
}
