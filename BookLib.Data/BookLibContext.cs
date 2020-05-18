using System;
using BookLib.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookLib.Data
{
    public class BookLibContext :DbContext
    {
        public BookLibContext(DbContextOptions<BookLibContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Queue> Queues { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User() { Id = 1, FullName="Admin",Login="Admin",Password="Admin",Role=Role.Admin });
        }
    }
}
