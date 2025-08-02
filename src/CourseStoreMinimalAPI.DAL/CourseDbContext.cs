using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.DAL
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
