using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = true;

        [Required]
        public int Age { get; set; }
        
        public List<hobby> Hobbies { get; set; }
    }
}
