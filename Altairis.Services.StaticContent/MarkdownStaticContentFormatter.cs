using Markdig;
using Microsoft.Extensions.Options;

namespace Altairis.Services.StaticContent;

public class MarkdownStaticContentFormatter : IStaticContentFormatter {
    private readonly IOptions<MarkdownStaticContentFormatterOptions> options;

    public MarkdownStaticContentFormatter(IOptions<MarkdownStaticContentFormatterOptions> options) {
        this.options = options;
    }

    public HtmlString GetHtml(string source) => new HtmlString(Markdown.ToHtml(source, this.options.Value.CreatePipeline()));

    public string GetPlainText(string source) => Markdown.ToPlainText(source, this.options.Value.CreatePipeline());
}

public class MarkdownStaticContentFormatterOptions {

    public Func<MarkdownPipeline> CreatePipeline { get; set; } = () => new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

}