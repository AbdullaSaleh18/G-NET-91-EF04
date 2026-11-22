
using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Entities;

namespace NationalBankManagementSystem.Data
{
    public class BankDbContext : DbContext
    {

        public BankDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=NationalBankDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys
            modelBuilder.Entity<Branch>()
                .HasKey(b => b.Code);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionNumber);

            modelBuilder.Entity<CustomerAccount>()
                .HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

            // Branch - Manager (One to One)
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Manager>(m => m.BranchCode);

            // Branch - Account (One to Many)
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Branch)
                .HasForeignKey(a => a.BranchCode);

            // Account - Transaction (One to Many)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountNumber);

            // Customer - CustomerAccount (One to Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CustomerAccounts)
                .WithOne(ca => ca.Customer)
                .HasForeignKey(ca => ca.CustomerId);

            // Account - CustomerAccount (One to Many)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.CustomerAccounts)
                .WithOne(ca => ca.Account)
                .HasForeignKey(ca => ca.AccountNumber);


        }

    }
}
