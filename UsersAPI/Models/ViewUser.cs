namespace UsersAPI.Models
{
    public class ViewUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; } = true;
        public int Age { get; set; }
        public List<String> Hobbies { get; set; }
    }
}
