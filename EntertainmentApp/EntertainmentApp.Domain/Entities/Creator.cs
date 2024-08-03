using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentApp.Domain.Entities
{
    public class Creator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation property
        public ICollection<Entertainment> Entertainments { get; set; }
    }
}
