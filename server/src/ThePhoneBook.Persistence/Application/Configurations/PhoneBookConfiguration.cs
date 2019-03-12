using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThePhoneBook.Core.Entities;

namespace ThePhoneBook.Persistence.Application.Configurations
{
    public class PhoneBookConfiguration : IEntityTypeConfiguration<PhoneBook>
    {
        public void Configure(EntityTypeBuilder<PhoneBook> builder)
        {
            builder.Property(pb => pb.Title).IsRequired().HasMaxLength(50);
            builder.Property(pb => pb.Description).IsRequired().HasMaxLength(100);
            builder.Property(pb => pb.UserId).IsRequired();
        }
    }
}