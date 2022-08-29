using Microsoft.AspNetCore.Html;

namespace Altairis.Services.StaticContent;
public class StaticContentProvider {
    private readonly IStaticContentStore store;
    private readonly IStaticContentFormatter formatter;
    private readonly ILogger<StaticContentProvider> logger;

    public StaticContentProvider(IStaticContentStore store, IStaticContentFormatter formatter, ILogger<StaticContentProvider> logger) {
        this.store = store;
        this.formatter = formatter;
        this.logger = logger;
    }

    public async Task<HtmlString> GetHtml(string key) {
        this.logger.LogDebug("Getting HTML for static page {key}.", key);
        return this.formatter.GetHtml(await this.store.GetSource(key));
    }

    public async Task<string> GetPlainText(string key) {
        this.logger.LogDebug("Getting plaintext for static page {key}.", key);
        return this.formatter.GetPlainText(await this.store.GetSource(key));
    }
}
