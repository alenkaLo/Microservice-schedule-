using Microsoft.EntityFrameworkCore;
using TimeTable.Models;
using TimeTable.Models.Entity;

namespace TimeTable.Data
{
    public class ApiContext : DbContext 
    {
        public DbSet<Lesson> Lessons { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
    }
}
