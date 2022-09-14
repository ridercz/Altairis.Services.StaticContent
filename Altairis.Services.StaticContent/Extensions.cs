using Microsoft.Extensions.DependencyInjection;

namespace Altairis.Services.StaticContent;

public static class Extensions {

    public static StaticContentBuilder AddStaticContent(this IServiceCollection services) {
        services.AddTransient<StaticContentProvider>();
        return new StaticContentBuilder(services);
    }

    public static StaticContentBuilder WithMarkdownFormatter(this StaticContentBuilder builder) {
        builder.Services.AddTransient<IStaticContentFormatter, MarkdownStaticContentFormatter>();
        return builder;
    }

    public static StaticContentBuilder WithFileStaticContentStore(this StaticContentBuilder builder, string dataFolder, string? fileExtension = null) {
        builder.Services.AddTransient<IStaticContentStore, FileStaticContentStore>();
        builder.Services.Configure<FileStaticContentStoreOptions>(options => {
            options.DataFolder = dataFolder;
            if (!string.IsNullOrWhiteSpace(fileExtension)) options.FileExtension = fileExtension;
        });
        return builder;
    }

    public static StaticContentBuilder WithDbStaticContentStore<TContext>(this StaticContentBuilder builder) where TContext : class, IStaticContentContext {
        builder.Services.AddTransient<IStaticContentStore, DbStaticContentStore>();
        builder.Services.AddTransient<IStaticContentContext, TContext>();
        return builder;
    }

}

public class StaticContentBuilder {
    public StaticContentBuilder(IServiceCollection services) {
        this.Services = services;
    }

    public IServiceCollection Services { get; init; }

}