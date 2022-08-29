namespace Altairis.Services.StaticContent;

public class DbStaticContentStore : IStaticContentStore {
    private readonly IStaticContentContext dc;
    private readonly ILogger<DbStaticContentStore> logger;

    public DbStaticContentStore(IStaticContentContext dc, ILogger<DbStaticContentStore> logger) {
        this.dc = dc;
        this.logger = logger;
    }

    public async Task<string> GetSource(string key) {
        var item = await this.dc.StaticContentItems.FindAsync(key);
        if (item == null) {
            this.logger.LogError("Static page with key {key} was not found in store.", key);
            return string.Empty;
        }
        return item.Text;
    }
}
