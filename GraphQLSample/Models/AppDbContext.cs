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

            for (short i = 1; i < 50; i++)
            {
                personBuilder.HasData(new Person { Id = i, FirstName = $"Alberto {i}", LastName = $"Monteiro {i}", Age = i });
            }
        }
    }
}
