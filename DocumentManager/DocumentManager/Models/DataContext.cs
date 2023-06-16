using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\Local;Database=DocumentManager;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
        }
       public DbSet<Role> Roles { get; set; }
       public DbSet<UserRole> UserRoles { get; set; }
       public DbSet<User> Users { get; set; }


    }
}
