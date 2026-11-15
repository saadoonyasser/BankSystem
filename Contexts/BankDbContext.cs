using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Contexts
{
    public class BankDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
     
               optionsBuilder.UseSqlServer(@"Server=.;Database=NationalBankManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True;");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);
        }
    }
}
