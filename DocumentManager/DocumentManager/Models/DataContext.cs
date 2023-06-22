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
            modelBuilder.Entity<UserDoc>()
                .HasKey(u => new { u.UserId, u.TypeId });

            modelBuilder.Entity<UserDoc>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDocs)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDoc>()
                .HasOne(ud => ud.Type)
                .WithMany(t => t.UserDocs)
                .HasForeignKey(ud => ud.TypeId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.IdCards)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<User>()
               .HasMany(u => u.UrbanCertificates)
               .WithOne(i => i.User)
               .HasForeignKey(i => i.UserId);
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<UserDoc> UserDocs { get; set; }
        public DbSet<IdCard> IdCards { get; set; }
        public DbSet<UrbanCertificate> UrbanCertificates { get; set;}


    }
}
