using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Altairis.Services.StaticContent.ViewComponents;

public class StaticContentViewComponent : ViewComponent {
    private readonly StaticContentProvider staticContentProvider;

    public StaticContentViewComponent(StaticContentProvider staticContentProvider) {
        this.staticContentProvider = staticContentProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(string key) {
        var html = await this.staticContentProvider.GetHtml(key);
        return new HtmlContentViewComponentResult(html);
    }

}
