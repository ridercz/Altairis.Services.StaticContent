﻿using Microsoft.Extensions.DependencyInjection;
using Olbrasoft.Extensions;

namespace Altairis.Services.StaticContent;
public class StaticContentBuilderExtensionsTests
{
    [Fact]
    public void Is_Public()
    {
        //Arrange
        var type = typeof(StaticContentBuilderExtensions);

        //Act
        var isPublic = type.IsPublic;

        //Assert
        Assert.True(isPublic);
    }

    [Fact]
    public void Is_Static()
    {
        //Arrange
        var type = typeof(StaticContentBuilderExtensions);

        //Act
        var isStatic = type.IsStatic();

        //Assert
        Assert.True(isStatic);
    }

    [Fact]
    public void UseDbStaticContentStoreFreeSql_Returns_SaticContenBuilder()
    {
        //Arrange
        var serviceCollectionMock = new Mock<IServiceCollection>();
        var BuilderMock = new Mock<StaticContentBuilder>(serviceCollectionMock.Object);

        //Act
        var result = StaticContentBuilderExtensions.UseDbStaticContentStoreFreeSql(BuilderMock.Object);

        //Assert
        Assert.IsAssignableFrom<StaticContentBuilder>(result);
    }


    [Fact]
    public void UseDbStaticContentStoreFreeSql_Add_DbStaticContentStoreFreeSql_To_ServiceCollection_As_IStaticContentStore()
    {
        //Arrange
        var services = new ServiceCollection();
        var freeSqlMock = new Mock<IFreeSql>();
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();
        IFreeSql freeSql = freeSqlMock.Object;

        var builder = new StaticContentBuilder(services); ;

        services.AddSingleton(freeSql);
        services.AddSingleton(loggerMock.Object);

        //Act
        StaticContentBuilderExtensions.UseDbStaticContentStoreFreeSql(builder);

        //Assert
        var store = services.BuildServiceProvider().GetService<IStaticContentStore>();
        Assert.IsType<DbStaticContentStoreFreeSql>(store);

    }
}
