using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Altairis.Services.StaticContent;

public class FileStaticContentStore : IStaticContentStore {
    private readonly IOptions<FileStaticContentStoreOptions> options;
    private readonly ILogger<FileStaticContentStore> logger;

    public FileStaticContentStore(IOptions<FileStaticContentStoreOptions> options, ILogger<FileStaticContentStore> logger) {
        this.options = options;
        this.logger = logger;
    }

    public async Task<string> GetSource(string key) {
        if (!Regex.IsMatch(key, "^[a-zA-Z0-9_-]{1,}$")) throw new ArgumentException("Invalid characters in key.", nameof(key));
        var fileName = Path.Combine(this.options.Value.DataFolder, key + this.options.Value.FileExtension);
        try {
            return await File.ReadAllTextAsync(fileName);
        } catch (IOException ioex) {
            this.logger.LogError(ioex, "Error while reading contents of file for page {key}.", key);
            return string.Empty;
        }
    }
}

public class FileStaticContentStoreOptions {

    public string DataFolder { get; set; } = "./App_Data/StaticContent";

    public string FileExtension { get; set; } = ".md";


}