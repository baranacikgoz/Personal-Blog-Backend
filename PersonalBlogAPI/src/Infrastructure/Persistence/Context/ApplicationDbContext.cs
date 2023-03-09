using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Common;
using Domain.Entities;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetValue<string>("ConnectionStrings:PersonalBlogDb"));
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableBaseEntity> entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        //entry.Entity.CreatedBy = _currentUser.Id;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        //entry.Entity.LastModifiedBy = _currentUser.Id;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        //entry.Entity.DeletedAtBy = _currentUser.Id;
                        entry.State = EntityState.Modified;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Article & Tag Many-to-Many Relationship
            _ = modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);

            _ = modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);

            _ = modelBuilder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });
            #endregion Article & Tag Many-to-Many Relationship

            #region Add Unique Indexes
            _ = modelBuilder.Entity<Article>()
                .HasIndex(a => a.Title)
                .IsUnique();

            _ = modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();
            #endregion Add Unique Indexes
        }
    }
}
