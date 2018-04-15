using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace MeetMusicModels.Models
{
    public abstract class ModelBase : IdentityUser
    {
        protected ModelBase()
        {
            Id = new Guid().ToString();
            UpdatedAt = DateTime.MinValue;
            DeletedAt = DateTime.MinValue;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string Id { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool Deleted { get; set; }
    }
}
