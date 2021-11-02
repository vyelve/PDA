using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PDADbContext _appDBContext;

        public UserRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _appDBContext.Users.ToListAsync();
        }
        public async Task<User> GetUserById(int Id)
        {
            return await _appDBContext.Users.FindAsync(Id);
        }

        public async Task<User> Add(User model)
        {
            _appDBContext.Users.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        
        public async Task<User> Update(User model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public bool Delete(int Id)
        {
            bool result = false;
            var _user = _appDBContext.Users.Find(Id);
            if (_user != null)
            {
                _appDBContext.Entry(_user).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool CheckUsernameExists(string loginName)
        {
            var result = _appDBContext.Users.Any(x => x.LoginName.Contains(loginName));
            return result;
        }

        public User GetUserDetailsByEmailId(string emailId)
        {
            return _appDBContext.Users.Where(whr => whr.EmailId == emailId).FirstOrDefault();
        }

        public User GetUserDetailsByLoginName(string loginName)
        {
            return _appDBContext.Users.Where(whr => whr.LoginName == loginName).FirstOrDefault();
        }
    }
}
