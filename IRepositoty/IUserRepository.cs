using Task4.Areas.Identity.Data;

namespace Task4.IRepositoty
{
    public interface IUserRepository
    {
        Task<AppUser[]> GetAllUsers();
        int BlockUsers(AppUser[] users);
        int UnblockUsers(AppUser[] users);
        int DeleteMany(AppUser[] users);
        AppUser GetUserById(string id);
        bool UpdateUser(AppUser user);
        bool DeleteById(string Id);
    }
}
