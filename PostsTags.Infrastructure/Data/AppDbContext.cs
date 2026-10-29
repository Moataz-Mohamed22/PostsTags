
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PostsTags.Core.Entities;

namespace PostsTags.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<PostTag> PostTags => Set<PostTag>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Post>(entity =>
        {
            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.Body)
                .IsRequired();

            entity.Property(p => p.CoverImagePath)
                .IsRequired();

            entity.Property(p => p.IsPinned)
                .IsRequired();

            entity.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(p => p.DeletedAt == null);
        });

        builder.Entity<Tag>(entity =>
        {
            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(t => t.Name)
                .IsUnique();
        });

        builder.Entity<PostTag>(entity =>
        {
            entity.HasKey(pt => new { pt.PostId, pt.TagId });

            entity.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
