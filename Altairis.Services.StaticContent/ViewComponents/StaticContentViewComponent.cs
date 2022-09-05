using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;

namespace Altairis.Services.StaticContent.ViewComponents;

public class StaticContentViewComponent : ViewComponent {
    private readonly StaticContentProvider staticContentProvider;
    private readonly IOptions<StaticContentViewComponentOptions> options;

    public StaticContentViewComponent(StaticContentProvider staticContentProvider, IOptions<StaticContentViewComponentOptions> options) {
        this.staticContentProvider = staticContentProvider;
        this.options = options;
    }

    public async Task<IViewComponentResult> InvokeAsync(string key) {
        // Get HTML itself
        var html = await this.staticContentProvider.GetHtml(key);
        if (string.IsNullOrEmpty(this.options.Value.SurroundingElementName)) return new HtmlContentViewComponentResult(html);

        // Create surrounding element
        var sb = new StringBuilder();
        sb.Append($"<{this.options.Value.SurroundingElementName}");
        foreach (var item in this.options.Value.SurroundingElementAttributes) {
            var encodedValue = HtmlEncoder.Default.Encode(item.Value);
            sb.Append($" {item.Key}=\"{encodedValue}\"");
        }
        sb.Append(">");
        sb.Append(html);
        sb.Append($"</{this.options.Value.SurroundingElementName}>");
        return new HtmlContentViewComponentResult(new HtmlString(sb.ToString()));
    }

}

public class StaticContentViewComponentOptions {

    public string? SurroundingElementName { get; set; }

    public IDictionary<string, string> SurroundingElementAttributes { get; set; } = new Dictionary<string, string>();

}