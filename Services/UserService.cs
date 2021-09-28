using IdentitySample.Models;

namespace IdentitySample.Services
{
    public class UserService: IUserService
    {
        private readonly IdentityDbContext context;

        public UserService(IdentityDbContext context)
        {
            this.context = context;
        }

        public User GetUserById(int id)
        {
            return context.Users.Find(id);
        }
    }

    public interface IUserService
    {
        public User GetUserById(int id);
    }
}