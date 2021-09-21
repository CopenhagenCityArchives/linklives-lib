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
                entity.HasKey(x => x.Key);
                entity.HasMany(x => x.Links).WithOne(x => x.LifeCourse).HasForeignKey(x => x.LifeCourseKey);
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(x => x.Key);
                entity.HasMany(x => x.Ratings).WithOne(x => x.Link).HasForeignKey(x => x.LinkKey);
            });

            modelBuilder.Entity<LinkRating>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Rating).WithMany().HasForeignKey(x => x.RatingId);
            });

            modelBuilder.Entity<RatingOption>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

        }
    }
}
