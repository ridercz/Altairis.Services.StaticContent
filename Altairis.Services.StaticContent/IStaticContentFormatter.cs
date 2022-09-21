namespace Altairis.Services.StaticContent;

public interface IStaticContentFormatter {

    HtmlString GetHtml(string source);

    string GetPlainText(string source);

}
