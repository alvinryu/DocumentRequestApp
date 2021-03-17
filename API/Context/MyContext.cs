using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DetailRequest> DetailRequests { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Person)
                .HasForeignKey<Account>(b => b.NIK);

            modelBuilder.Entity<Person>()
                .HasMany(c => c.Requests)
                .WithOne(e => e.Person);

            modelBuilder.Entity<Department>()
                .HasMany(c => c.People)
                .WithOne(e => e.Department);

            modelBuilder.Entity<Request>()
                .HasOne(a => a.DetailRequest)
                .WithOne(b => b.Request)
                .HasForeignKey<Request>(b => b.RequestID);

            modelBuilder.Entity<DocumentType>()
                .HasMany(c => c.Requests)
                .WithOne(e => e.DocumentType);

            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new { ar.NIK, ar.RoleID });

            modelBuilder.Entity<AccountRole>()
                .HasOne(a => a.Account)
                .WithMany(ar => ar.AccountRoles)
                .HasForeignKey(a => a.NIK);

            modelBuilder.Entity<AccountRole>()
                .HasOne(r => r.Role)
                .WithMany(ar => ar.AccountRoles)
                .HasForeignKey(r => r.RoleID);
        }
    }
}
