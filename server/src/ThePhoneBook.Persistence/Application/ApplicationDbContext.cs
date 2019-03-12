using Microsoft.EntityFrameworkCore;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Persistence.Application.Configurations;

namespace ThePhoneBook.Persistence.Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PhoneBook> PhoneBooks { get; set; }
        public DbSet<PhoneBookEntry> PhoneBookEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneBookConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneBookEntryConfiguration());
        }
    }
}