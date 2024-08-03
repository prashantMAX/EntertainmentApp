
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntertainmentApp.Domain.Entities
{
    public class UserLike 
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Review Review { get; set; }
    }
}
