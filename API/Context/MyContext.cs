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
                .HasOne(person => person.Account)
                .WithOne(account => account.Person)
                .HasForeignKey<Account>(account => account.NIK);

            modelBuilder.Entity<Person>()
                .HasMany(person => person.Requests)
                .WithOne(request => request.Person);

            modelBuilder.Entity<Department>()
                .HasMany(department => department.People)
                .WithOne(person => person.Department);

            modelBuilder.Entity<DetailRequest>()
                .HasOne(detailrequest => detailrequest.Request)
                .WithOne(request => request.DetailRequest)
                .HasForeignKey<DetailRequest>(detailrequest => detailrequest.RequestID);

            modelBuilder.Entity<DocumentType>()
                .HasMany(documenttype => documenttype.Requests)
                .WithOne(request => request.DocumentType);

            modelBuilder.Entity<AccountRole>()
                .HasKey(accountrole => new { accountrole.NIK, accountrole.RoleID });

            modelBuilder.Entity<AccountRole>()
                .HasOne(accountrole => accountrole.Account)
                .WithMany(account => account.AccountRoles)
                .HasForeignKey(accountrole => accountrole.NIK);

            modelBuilder.Entity<AccountRole>()
                .HasOne(accountrole => accountrole.Role)
                .WithMany(role => role.AccountRoles)
                .HasForeignKey(accountrole => accountrole.RoleID);
        }
    }
}
