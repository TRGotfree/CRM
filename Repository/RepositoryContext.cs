using CRM.Models;
using CRM.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repository
{
    public class RepositoryContext : DbContext
    {
        private readonly IHashGenerator hashGenerator;

        public RepositoryContext(IHashGenerator hashGenerator, DbContextOptions<RepositoryContext> options) : base(options)
        {
            this.hashGenerator = hashGenerator;
            Database.EnsureCreated();
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<ClientType> ClientType { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);

        }
    }
}
