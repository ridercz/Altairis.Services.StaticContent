using Markdig;
using Microsoft.AspNetCore.Html;

namespace Altairis.Services.StaticContent;

public class MarkdownStaticContentFormatter : IStaticContentFormatter {
    public HtmlString GetHtml(string source) {
        return new HtmlString(Markdown.ToHtml(source));
    }

    public string GetPlainText(string source) {
        return Markdown.ToPlainText(source);
    }
}
