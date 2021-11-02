using PDA.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface IUserRepository 
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int Id);
        Task<User> Add(User model);
        Task<User> Update(User model);
        bool Delete(int Id);

        bool CheckUsernameExists(string emailId);
        User GetUserDetailsByEmailId(string emailId);
        User GetUserDetailsByLoginName(string loginName);
    }
}
