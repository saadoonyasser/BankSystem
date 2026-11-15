using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(m => m.BranchCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasIndex(m => m.BranchCode)
                .IsUnique();

            builder.HasOne(m => m.Branch)
                .WithOne(b => b.Manager)
                .HasForeignKey<Manager>(m => m.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
