using Microsoft.EntityFrameworkCore;

namespace Altairis.Services.StaticContent;

public interface IStaticContentContext {

    public DbSet<StaticContentItem> StaticContentItems { get; }

}
