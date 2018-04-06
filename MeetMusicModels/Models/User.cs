using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetMusicModels.Models
{
    public class User : ModelBase
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must contains between {2} & {1} characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]{2,40}$",
        ErrorMessage = "Firstname must be at least 2 characters and not exceed 40 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]{2,40}$",
        ErrorMessage = "Lastname must be at least 2 characters and not exceed 40 characters")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
        ErrorMessage = "Not a mail adress")]
        [StringLength(254, ErrorMessage = "Email must not exceed {1} characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-2]$", ErrorMessage = "Gender must be an integer between 1 and 2")]
        public int Gender { get; set; }

        public string AvatarUrl { get; set; }

        [RegularExpression(@"^(0|\+33)[1-9]([-. ]?[0-9]{2}){4}$",
        ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [StringLength(500, ErrorMessage = "Description is limited to {1} characters.")]
        public string Description { get; set; }

        public ICollection<UserMusicFamily> UserMusicFamilies { get; set; }
    }
}
