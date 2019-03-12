using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.Persistence.Application;

namespace ThePhoneBook.Persistence.Repositories
{
    /// <summary>
    /// The user repository
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}