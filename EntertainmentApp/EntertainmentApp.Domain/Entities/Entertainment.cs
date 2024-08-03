using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentApp.Domain.Entities
{
    public class Entertainment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(10000)]
        public string Description { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public int GenreId { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public EntertainmentCategory Category { get; set; }

       
        public string? ImageURL { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public Creator Creator { get; set; }
        public Genre Genre { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
