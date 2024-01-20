
using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;

namespace UsersAPI.Repository
{
    public class UsersDAL: IUsersDAL
    {
        private readonly userContext _userContext;
        public UsersDAL(userContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<User> ValidateUser(LoginRequest request)
        {
            return await Task.Run(() => _userContext.Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password )); 
        }

        public async Task<List<ViewUser>> GetUsers()
        {
            var users = _userContext.Users.Select(u => new ViewUser
            {
                Id = u.Id,
                Username = u.Username,
                IsAdmin = u.IsAdmin,
                Age = u.Age,
                Hobbies = u.Hobbies.Select(hobby => hobby.value).ToList()

            }).ToList();
            return await Task.Run(() => users);
        }
        public async Task<ViewUser> GetUser(Guid id)
        {
            var user = _userContext.Users.Select(u => new ViewUser
            {
                Id = u.Id,
                Username = u.Username,
                IsAdmin = u.IsAdmin,
                Age = u.Age,
                Hobbies = u.Hobbies.Select(hobby => hobby.value).ToList()

            }).FirstOrDefault(u => u.Id == id);
            return await Task.Run(() => user);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user =  _userContext.Users.Include(user => user.Hobbies).ToList().FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _userContext.Hobbies.RemoveRange(user.Hobbies);
                _userContext.Users.Remove(user);
                _userContext.SaveChanges();
                return await Task.Run(() => true);
            }
            else
            {
                return await Task.Run(() => false);
            }
        }
        public async Task<Guid> AddUsers(AddUpdateUser user)
        {
            var newID = Guid.NewGuid();
            var Hobbies = new List<hobby>();
            foreach (string hobby in user.Hobbies)
            {
                hobby newHobby = new hobby
                {
                    value = hobby
                };
                Hobbies.Add(newHobby);
            }
            var newUser = new User
            {
                Id = newID,
                Username = user.Username,
                Password = user.Password,
                IsAdmin = user.IsAdmin,
                Age = user.Age,
                Hobbies = Hobbies
            };
            _userContext.Users.Add(newUser);
            _userContext.SaveChanges();
            return await Task.Run(() => newID);
        }
        public async Task<bool> UpdateUsers(Guid id, AddUpdateUser user)
        {
            var userToUpdate = _userContext.Users.Include(user => user.Hobbies).ToList().FirstOrDefault(u => u.Id == id);
            var Hobbies = new List<hobby>();
            foreach (string hobby in user.Hobbies)
            {
                hobby newHobby = new hobby
                {
                    value = hobby
                };
                Hobbies.Add(newHobby);
            }
            userToUpdate.Username = user.Username;
            userToUpdate.Password = user.Password;
            userToUpdate.Age = user.Age;
            userToUpdate.Hobbies = Hobbies;
            _userContext.SaveChanges();
            return await Task.Run(() => true);
        }
        public async Task<bool> checkExistUserName(string userName, Guid? id)
        {
            if (id==null)
            {
                return await Task.Run(() => _userContext.Users.Select(u => u.Username).Contains(userName));
            }
            else
            {
                return await Task.Run(() => _userContext.Users.Where(u => u.Id != id).Select(u => u.Username).Contains(userName));
            }
        }
    }
}
