using FreeSql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altairis.Services.StaticContent;
public class DbStaticContentStoreFreeSqlTests
{
    [Fact]
    public void Is_Public()
    {
        //Arrange
        var type = typeof(DbStaticContentStoreFreeSql);

        //Act
        var isPublic = type.IsPublic;

        //Assert
        Assert.True(isPublic);

    }

    [Fact]
    public void Implemented_Interface_IStaticContentStore()
    {
        //Arrange
        var type = typeof(IStaticContentStore);

        //Act
        DbStaticContentStoreFreeSql strore = CreateStore();

        //Assert
        Assert.IsAssignableFrom(type, strore);
    }

    private static DbStaticContentStoreFreeSql CreateStore()
    {
        var freeSqlMock = new Mock<IFreeSql>();
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();

        return new DbStaticContentStoreFreeSql(freeSqlMock.Object, loggerMock.Object);
    }

    [Fact]
    public void Throw_ArgumentNullException_If_FreeSql_Is_Null()
    {
        //Arrange
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();

        //Act

        //Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new DbStaticContentStoreFreeSql(null, loggerMock.Object));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void Throw_ArgumentNullException_If_Logger_Is_Null()
    {
        //Arrange
        var freeSqlMock = new Mock<IFreeSql>();

        //Act

        //Assert

        Assert.Throws<ArgumentNullException>(() => new DbStaticContentStoreFreeSql(freeSqlMock.Object, null));
    }


    [Fact]
    public async void GetSource_Calls_Log_If_Item_Key_Not_Exists()
    {
        //Arrange
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();
        var freeSqlMock = CreateNotExistKeyFreeSqlMock();
        var store = new DbStaticContentStoreFreeSql(freeSqlMock.Object, loggerMock.Object);

        //Act
        await store.GetSource("NotExistKey");

        //Assert
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        loggerMock.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
         It.IsAny<EventId>(),
         It.Is<It.IsAnyType>((v, t) => true),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
     Times.Once);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

    }

    [Fact]
    public async Task GetSource_Returns_String_Empty_If_Key_Not_ExistsAsync()
    {
        //Arrange
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();
        var freeSqlMock = CreateNotExistKeyFreeSqlMock();
        var store = new DbStaticContentStoreFreeSql(freeSqlMock.Object, loggerMock.Object);

        //Act
        var result = await store.GetSource("NotExistKey");

        //Assert
        Assert.True(string.IsNullOrEmpty(result));
    }

    [Fact]
    public async Task GetSource_returns_AwesomeValue_If_Key_AwesomeKeyAsync()
    {
        //Arrange
        var loggerMock = new Mock<ILogger<DbStaticContentStoreFreeSql>>();
        var freeSqlMock = new Mock<IFreeSql>();
        var whereSelectMock = new Mock<ISelect<StaticContentItem>>();
        var toListSelectMock = new Mock<ISelect<StaticContentItem>>();

        freeSqlMock.Setup(p => p.Select<StaticContentItem>()).Returns(whereSelectMock.Object);
        whereSelectMock.Setup(m => m.Where(p => p.Key == "AwesomeKey")).Returns(toListSelectMock.Object);
        toListSelectMock.Setup(p => p.ToListAsync(default)).Returns(Task.FromResult(new List<StaticContentItem>
        { new StaticContentItem {Text="AwesomeValue"} }));
        var store = new DbStaticContentStoreFreeSql(freeSqlMock.Object, loggerMock.Object);

        //Act
        var result = await store.GetSource("AwesomeKey");

        //Assert
        Assert.True(result == "AwesomeValue");
    }


    Mock<IFreeSql> CreateNotExistKeyFreeSqlMock()
    {
        var freeSqlMock = new Mock<IFreeSql>();
        var whereSelectMock = new Mock<ISelect<StaticContentItem>>();
        var toListSelectMock = new Mock<ISelect<StaticContentItem>>();


        freeSqlMock.Setup(p => p.Select<StaticContentItem>()).Returns(whereSelectMock.Object);
        whereSelectMock.Setup(m => m.Where(p => p.Key == "NotExistKey")).Returns(toListSelectMock.Object);
        toListSelectMock.Setup(p => p.ToListAsync(default)).Returns(Task.FromResult(new List<StaticContentItem>()));

        return freeSqlMock;
    }


}
