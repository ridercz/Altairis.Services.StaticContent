using Microsoft.EntityFrameworkCore;

namespace Altairis.Services.StaticContent.DemoApp.Data;

public class DemoDbContext : DbContext, IStaticContentContext {

    public DemoDbContext(DbContextOptions options) : base(options) {
    }

    public DbSet<StaticContentItem> StaticContentItems => this.Set<StaticContentItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StaticContentItem>().HasData(
            new StaticContentItem { Key = "welcome", Text = "# Welcome" },
            new StaticContentItem { Key = "privacy", Text = "This is my _privacy policy_." }
        );
    }
}
