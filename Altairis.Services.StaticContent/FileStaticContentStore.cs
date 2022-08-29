using System.Text.RegularExpressions;

namespace Altairis.Services.StaticContent;

public class FileStaticContentStore : IStaticContentStore {
    private const string DataFolder = "./App_Data/StaticContent";
    private readonly ILogger<FileStaticContentStore> logger;

    public FileStaticContentStore(ILogger<FileStaticContentStore> logger) {
        this.logger = logger;
    }

    public async Task<string> GetSource(string key) {
        if (!Regex.IsMatch(key, "^[a-zA-Z0-9_-]{1,}$")) throw new ArgumentException("Invalid characters in key.", nameof(key));
        var fileName = Path.Combine(DataFolder, key + ".md");
        try {
            return await File.ReadAllTextAsync(fileName);
        } catch (IOException ioex) {
            this.logger.LogError(ioex, "Error while reading contents of file for page {key}.", key);
            return string.Empty;
        }
    }
}
