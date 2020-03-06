using CRM.Models;
using CRM.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Constants;

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
        public DbSet<Phone> Phone { get; set; }
        public DbSet<UserTaskType> UserTaskType { get; set; }
        public DbSet<UserTaskState> UserTaskState { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Payload> Payload { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Login = "admin", Password = hashGenerator.GetHash("12345"), Name = "Аdministrator", UserRoleId = 1, IsActive = true });

            modelBuilder.Entity<UserRole>().HasKey(r => r.Id);
            modelBuilder.Entity<UserRole>().Property(r => r.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = 1, Name = "Аdministrator" });

            modelBuilder.Entity<City>().HasKey(c => c.Id);
            modelBuilder.Entity<City>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<City>().Property(c => c.IsActive).HasDefaultValue(true);

            modelBuilder.Entity<ClientType>().HasKey(ct => ct.Id);
            modelBuilder.Entity<ClientType>().Property(ct => ct.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ClientType>().HasIndex(ct => ct.Name).IsUnique();

            modelBuilder.Entity<Phone>().HasKey(p => p.Id);
            modelBuilder.Entity<Phone>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Phone>().HasIndex(p => new { p.Number, p.ClientId });

            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Client>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Client>().Property(c => c.CreationDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Client>().Property(c => c.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Client>().HasIndex(c => c.ClientTypeId);

            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Employee>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>().HasIndex(e => new { e.ClientId, e.Name, e.Position }).IsUnique();

            modelBuilder.Entity<UserTaskType>().HasKey(u => u.Id);
            modelBuilder.Entity<UserTaskType>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserTaskType>().HasIndex(u => u.Name).IsUnique();

            modelBuilder.Entity<UserTaskState>().HasKey(u => u.Id);
            modelBuilder.Entity<UserTaskState>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserTaskState>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<UserTaskState>().HasData(
                new UserTaskState { Id = (int)UserTaskStates.New, Name = "Новая задача" },
                new UserTaskState { Id = (int)UserTaskStates.Processing, Name = "Задача выполняется" },
                new UserTaskState { Id = (int)UserTaskStates.Proceed, Name = "Задача выполнена" },
                new UserTaskState { Id = (int)UserTaskStates.Canceled, Name = "Задача отменена" }
            );

            modelBuilder.Entity<Models.Priority>().HasKey(p => p.Id);
            modelBuilder.Entity<Models.Priority>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.Priority>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Models.Priority>().HasData(
                new Models.Priority { Id = (int)Constants.Priority.Normal, Name = "Обычный" }, 
                new Models.Priority { Id = (int)Constants.Priority.High, Name = "Высокий" }, 
                new Models.Priority { Id = (int)Constants.Priority.Urgent, Name = "Срочный" });

            modelBuilder.Entity<Payload>().HasKey(p => p.Id);
            modelBuilder.Entity<Payload>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserTask>().HasKey(u => u.Id);
            modelBuilder.Entity<UserTask>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserTask>().Property(u => u.OpenDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<UserTask>().Property(u => u.ChangeDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<UserTask>().Property(u => u.PriorityId).HasDefaultValue(1);
            modelBuilder.Entity<UserTask>().Property(u => u.UserTaskStateId).HasDefaultValue(1);
            modelBuilder.Entity<UserTask>().HasIndex(u => u.UserTaskStateId);
            modelBuilder.Entity<UserTask>().HasIndex(u => u.UserTaskTypeId);
            modelBuilder.Entity<UserTask>().HasIndex(u => u.PriorityId);
        }
    }
}
