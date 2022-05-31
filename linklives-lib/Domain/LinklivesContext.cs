using Microsoft.EntityFrameworkCore;

namespace Linklives.Domain
{
    public class LinklivesContext : DbContext
    {
        public DbSet<LifeCourse> LifeCourses { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<LinkRating> LinkRatings { get; set; }
        public DbSet<RatingOption> RatingOptions { get; set; }
        
        public LinklivesContext(DbContextOptions<LinklivesContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LifeCourse>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Links).WithMany(x => x.LifeCourses);
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Ratings).WithOne(x => x.Link).HasForeignKey(x => x.LinkId);
            });

            modelBuilder.Entity<LinkRating>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Rating).WithMany().HasForeignKey(x => x.RatingId);
                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<RatingOption>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<RatingOption>().HasData(
                new RatingOption() { Id = 1, Text = "Det ser fornuftigt ud. Personinformationen i de to kilder passer sammen.", Heading = "Ja, det er troværdigt", Category = "positive" },
                new RatingOption() { Id = 2, Text = "Jeg kan bekræfte informationen fra andre kilder, der ikke er med i Link-Lives.", Heading = "Ja, det er troværdigt", Category = "positive" },
                new RatingOption() { Id = 3, Text = "Jeg kan genkende informationen fra min private slægtsforskning.", Heading = "Ja, det er troværdigt", Category = "positive" },
                new RatingOption() { Id = 4, Text = "Det ser forkert ud. Personinformation i de to kilder passer ikke sammen.", Heading = "Nej, det er ikke troværdigt", Category = "negative" },
                new RatingOption() { Id = 5, Text = "Jeg ved det er forkert ud fra andre kilder, der ikke er med i Link-Lives.", Heading = "Nej, det er ikke troværdigt", Category = "negative" },
                new RatingOption() { Id = 6, Text = "Jeg ved det er forkert fra min private slægtsforskning.", Heading = "Nej, det er ikke troværdigt", Category = "negative" },
                new RatingOption() { Id = 7, Text = "Jeg er i tvivl om personinformationen i de to kilder passer sammen.", Heading = "Måske", Category = "neutral" },
                new RatingOption() { Id = 8, Text = "Nogle af informationerne passer sammen. Andre gør ikke.", Heading = "Måske", Category = "neutral" },
                new RatingOption() { Id = 9, Text = "Der er ikke personinformation nok til at afgøre, om det er troværdigt.", Heading = "Måske", Category = "neutral" }
            );
        }
    }
}
