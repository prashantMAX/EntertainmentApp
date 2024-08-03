using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EntertainmentApp.Domain.Entities
{
    public class Review 
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EntertainmentId { get; set; }
        public string? UserId { get; set; }
        public string? reviewerName {  get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Entertainment? Entertainment { get; set; }
        public virtual ICollection<UserLike>? UserLikes { get; set; } = new List<UserLike>();
    }
}
