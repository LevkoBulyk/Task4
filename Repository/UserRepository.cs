using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Task4.Areas.Identity.Data;
using Task4.Areas.Identity.Data.Enum;
using Task4.Data;
using Task4.IRepositoty;

namespace Task4.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public int BlockUsers(AppUser[] users)
        {
            int res = 0;
            foreach (var user in users)
            {
                if (user.CheckBox && user.Status == UserStatus.Active)
                {
                    var u = GetUserById(user.Id);
                    u.Status = UserStatus.Blocked;
                    res += UpdateUser(u) ? 1 : 0;
                }
            }
            return res;
        }

        public bool DeleteById(string Id)
        {
            var user = GetUserById(Id);
            _userDbContext.Remove(user);
            return _userDbContext.SaveChanges() > 0;
        }

        public int DeleteMany(AppUser[] users)
        {
            int res = 0;
            foreach (var user in users)
            {
                if (user.CheckBox)
                {
                    res += DeleteById(user.Id) ? 1 : 0;
                }
            }
            return res;
        }

        public async Task<AppUser[]> GetAllUsers()
        {
            return await _userDbContext.AppUsers.AsNoTracking().ToArrayAsync();
        }

        public AppUser GetUserById(string id)
        {
            return _userDbContext.AppUsers.FirstOrDefault(u => u.Id.Equals(id));
        }

        public int UnblockUsers(AppUser[] users)
        {
            int res = 0;
            foreach (var user in users)
            {
                if (user.CheckBox && user.Status == UserStatus.Blocked)
                {
                    var u = GetUserById(user.Id);
                    u.Status = UserStatus.Active;
                    res += UpdateUser(u) ? 1 : 0;
                }
            }
            return res;
        }

        public bool UpdateUser(AppUser user)
        {
            _userDbContext.AppUsers.Update(user);
            return _userDbContext.SaveChanges() > 0;
        }
    }
}
