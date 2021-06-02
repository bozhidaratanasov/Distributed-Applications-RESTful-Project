using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Context
{
   public class ApplicationDbContext : DbContext
    {
        public DbSet<Fragrance> Fragrances { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext() : base()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Fragrances = this.Set<Fragrance>();
            Customers = this.Set<Customer>();
            Sales = this.Set<Sale>();
            Users = this.Set<User>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database = FragranceDb; User Id = bozhidar; Password = mypass;");
        }

    }
}
