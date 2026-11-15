using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.AccountNumber);

            builder.Property(a => a.AccountNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(a => a.CurrentBalance)
                .HasPrecision(18, 2);

            builder.Property(a => a.AccountType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.BranchCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
