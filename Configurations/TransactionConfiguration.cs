using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionNumber);

            builder.Property(t => t.TransactionNumber)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Amount)
                .HasPrecision(18, 2);

            builder.Property(t => t.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(t => t.Note)
                .HasMaxLength(250);

            builder.Property(t => t.AccountNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
