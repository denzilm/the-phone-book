using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThePhoneBook.Core.Entities;

namespace ThePhoneBook.Persistence.Application.Configurations
{
    public class PhoneBookEntryConfiguration : IEntityTypeConfiguration<PhoneBookEntry>
    {
        public void Configure(EntityTypeBuilder<PhoneBookEntry> builder)
        {
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
        }
    }
}