using Microsoft.EntityFrameworkCore;

namespace UsersAPI.Models
{
    public class userContext:DbContext
    {
        public userContext(DbContextOptions options): base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<hobby> Hobbies { get; set; }

    }
}
