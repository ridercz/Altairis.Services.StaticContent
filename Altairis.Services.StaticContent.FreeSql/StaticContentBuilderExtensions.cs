using Microsoft.Extensions.DependencyInjection;

namespace Altairis.Services.StaticContent;

public static class StaticContentBuilderExtensions
{
    public static StaticContentBuilder UseDbStaticContentStoreFreeSql(this StaticContentBuilder builder)
    {
        builder.Services.AddTransient<IStaticContentStore, DbStaticContentStoreFreeSql>();

        return builder;
    }
}