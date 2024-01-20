using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class AddUpdateUser
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = true;

        [Required]
        public int Age { get; set; }

        public List<String> Hobbies { get; set; }
    }
}
