using UsersAPI.Models;

namespace UsersAPI.Repository
{
    public interface IUsersDAL
    {
        public Task<User> ValidateUser(LoginRequest request);
        public Task<List<ViewUser>> GetUsers();
        public Task<ViewUser> GetUser(Guid id);
        public Task<bool> DeleteUser(Guid id);
        public Task<Guid> AddUsers(AddUpdateUser user);
        public Task<bool> UpdateUsers(Guid id, AddUpdateUser user);
        public Task<bool> checkExistUserName(string userName, Guid? id = null);
    }
}
