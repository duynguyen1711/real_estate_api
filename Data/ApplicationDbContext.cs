using Microsoft.EntityFrameworkCore;
using real_estate_api.Models;

namespace real_estate_api.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostDetail> PostDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mối quan hệ giữa Post và PostDetail (1 - 1)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.PostDetail)
                .WithOne(pd => pd.Post)
                .HasForeignKey<PostDetail>(pd => pd.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa User và SavedPost (1 - N)
            modelBuilder.Entity<SavedPost>()
                .HasOne(sp => sp.User)
                .WithMany(u => u.SavedPosts)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Mối quan hệ giữa Post và SavedPost (1 - N)
            modelBuilder.Entity<SavedPost>()
                .HasOne(sp => sp.Post)
                .WithMany(p => p.SavedPosts)
                .HasForeignKey(sp => sp.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            // Đảm bảo userId và postId là duy nhất trong SavedPost
            modelBuilder.Entity<SavedPost>()
                .HasIndex(sp => new { sp.UserId, sp.PostId })
                .IsUnique();

            // Mối quan hệ giữa User và Post (1 - N)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình Enum Type cho Post
            modelBuilder.Entity<Post>()
                .Property(p => p.Type)
                .HasConversion<string>();

            // Cấu hình Enum Property cho Post
            modelBuilder.Entity<Post>()
                .Property(p => p.Property)
                .HasConversion<string>();
            base.OnModelCreating(modelBuilder);
        }
        }
}
