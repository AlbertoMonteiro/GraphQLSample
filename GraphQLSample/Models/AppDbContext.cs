using Microsoft.EntityFrameworkCore;

namespace GraphQLSample.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var personBuilder = modelBuilder.Entity<Person>();
            personBuilder
                .Property(x => x.FirstName).IsRequired().HasMaxLength(50).IsUnicode(false);
            personBuilder
                .Property(x => x.LastName).IsRequired().HasMaxLength(50).IsUnicode(false);

            personBuilder.HasData(new Person { Id = 1, FirstName = "Alberto", LastName = "Monteiro", Age = 31 });
        }
    }
}
