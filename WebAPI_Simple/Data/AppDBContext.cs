using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebAPI_Simple.Models.Domain;

namespace WebAPI_Simple.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> DbContextOptions) : base (DbContextOptions) 
        {
            // constructor
        }

        //define model
        protected override void OnModelCreating (ModelBuilder modelbuilder)
        {
            // can use Fluent API to define relationship between tables 
            modelbuilder.Entity<Books_Authors>()
                .HasOne(b => b.Books)
                .WithMany(ba => ba.Books_Authors)
                .HasForeignKey(bi => bi.BookId);
            modelbuilder.Entity<Books_Authors>()
                .HasOne(b => b.Authors)
                .WithMany(ba => ba.Books_Authors)
                .HasForeignKey(bi => bi.AuthorId);
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet <Publishers> Publishers { get; set; }
        public DbSet <Books_Authors> Books_Authors { get; set; }
    }
}
