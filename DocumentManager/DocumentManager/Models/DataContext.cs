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
            modelBuilder.Entity<User>()
               .HasMany(u => u.LandCertificates)
               .WithOne(i => i.User)
               .HasForeignKey(i => i.UserId);
            modelBuilder.Entity<User>()
               .HasMany(u => u.CadastralPlans)
               .WithOne(i => i.User)
               .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<UserGeneratedDoc>()
                .HasKey(ugd => new { ugd.UserId, ugd.TypeId });

            modelBuilder.Entity<UserGeneratedDoc>()
                .HasOne(ugd => ugd.User)
                .WithMany(u => u.UserGeneratedDocs)
                .HasForeignKey(ugd => ugd.UserId);

            modelBuilder.Entity<UserGeneratedDoc>()
                .HasOne(ugd => ugd.Type)
                .WithMany(t => t.UserGeneratedDocs)
                .HasForeignKey(ugd => ugd.TypeId);
            ;

            modelBuilder.Entity<UserStatus>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserStatuses) // Assuming that you have UserStatuses collection in your User model
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserStatus>()
                .HasOne(us => us.FeedbackStatus)
                .WithMany()
                .HasForeignKey(us => us.FeddbackId);

            modelBuilder.Entity<UserStatus>()
                .HasOne(us => us.AuthorizationStatus)
                .WithMany()
                .HasForeignKey(us => us.AuthorizationId);

            modelBuilder.Entity<UserStatus>()
                .HasOne(us => us.EngineeringStatus)
                .WithMany()
                .HasForeignKey(us => us.EngineeringId);
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<UserDoc> UserDocs { get; set; }
        public DbSet<IdCard> IdCards { get; set; }
        public DbSet<UrbanCertificate> UrbanCertificates { get; set;}
        public DbSet<LandCertificate> LandCertificates { get; set; }
        public DbSet<CadastralPlan> CadastralPlans { get; set;}
        public DbSet<GenerateDocType> GenerateDocTypes { get; set; }
        public DbSet<UserGeneratedDoc> UserGeneratedDocs { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<FeedbackStatus> FeedbackStatuses { get; set; }
        public DbSet<AuthorizationStatus> AuthorizationStatuses { get; set; }
        public DbSet<EngineeringStatus> EngineeringStatuses { get; set; }


    }
}
