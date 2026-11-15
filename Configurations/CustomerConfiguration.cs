using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.NationalId)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(c => c.NationalId)
                .IsUnique();

            builder.Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Address)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.CustomerType)
                .HasConversion<string>()
                .HasMaxLength(20);
        }
    }
}
