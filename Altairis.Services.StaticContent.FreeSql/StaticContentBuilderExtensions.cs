using Microsoft.Extensions.DependencyInjection;

namespace Altairis.Services.StaticContent;

public static class StaticContentBuilderExtensions
{
    public static object UseDbStaticContentStoreFreeS( this StaticContentBuilder builder)
    {
        builder.Services.AddTransient<IStaticContentStore, DbStaticContentStoreFreeSql>();

        return builder;
    }
}