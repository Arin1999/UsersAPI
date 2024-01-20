using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class hobby
    {
        [Key]
        public int id { get; set; }
        public string value { get; set; }
    }
}
