using Microsoft.EntityFrameworkCore;
using TimeTable.Configurations;
using TimeTable.Models;
using TimeTable.Models.Entity;

namespace TimeTable.Data
{
    public class LessonDbContext : DbContext 
    {
        public DbSet<Lesson> Lessons { get; set; }

        public LessonDbContext(DbContextOptions<LessonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LessonConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
