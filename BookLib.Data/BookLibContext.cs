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
        public DbSet<Library> Libraries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User() { Id = 1, FullName="Admin",Login="Admin",Password="Admin",Role=Role.Admin });
            modelBuilder.Entity<Library>().HasData(new Library() { Id = 1, Name = "Библиотека им. Пушкина", Address = "ул. Крылатых Скворцов 71"});
            modelBuilder.Entity<Library>().HasData(new Library() { Id = 2, Name = "Национальная Библиотека", Address = "ул. Татарстана 17" });
        }
    }
}
