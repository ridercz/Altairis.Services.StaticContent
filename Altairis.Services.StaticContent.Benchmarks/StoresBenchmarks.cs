using Altairis.Services.StaticContent.DemoApp.Data;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Altairis.Services.StaticContent.Benchmarks;


public class Orm
{
    public static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.SqlServer,
               "Server=.;Database=Test;Trusted_Connection=True;MultipleActiveResultSets=true",
                typeof(FreeSql.SqlServer.SqlServerProvider<>))
            //.UseConnectionString(FreeSql.DataType.MySql, "Data Source=127.0.0.1;Port=3306;User ID=root;Password=root;Initial Catalog=cccddd;Charset=utf8;SslMode=none;Max pool size=20")
            .UseAutoSyncStructure(false)
            .UseNoneCommandParameter(true)
            //.UseConfigEntityFromDbFirst(true)
            .Build();
}

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StoresBenchmarks
{

    IStaticContentStore _dbStaticContentStore;
    DbStaticContentStoreFreeSql _dbStaticContentStoreFreeSql;
    IFreeSql _fsql;


    [GlobalSetup]
    public void Setup()
    {
       //Orm.fsql.CodeFirst.ConfigEntity<StaticContentItem>(a =>
       //  {
       //      a.Name("StaticContentItems");
       //      a.Property(p => p.Key).Name("Key").IsPrimary(true);
       //      a.Property(p => p.Text).Name("Text");
       //  });


        var connectionString = "Data Source=C:\\Olbrasoft\\Altairis\\Altairis.Services.StaticContent\\Altairis.Services.StaticContent.Benchmarks\\App_Data\\StaticContent.db";
        //var connectionString = "Server=.;Database=Test;Trusted_Connection=True;MultipleActiveResultSets=true";

         _fsql = new FreeSql.FreeSqlBuilder()
                                      .UseConnectionString(FreeSql.DataType.Sqlite, connectionString)
                                      //Automatically synchronize the entity structure to the database.
                                      //FreeSql will not scan the assembly, and will generate a table if and only when the CRUD instruction is executed.
                                      .UseAutoSyncStructure(false)
                                      .Build();
        
        _fsql.CodeFirst // https://www.cnblogs.com/FreeSql/p/11531302.html
            .ConfigEntity<StaticContentItem>(a =>
            {
                a.Name("StaticContentItems");
                a.Property(p => p.Key).Name( "Key").IsPrimary(true);
                a.Property(p => p.Text).Name("Text");
            });

        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddLogging();
               
        serviceCollection.AddDbContext<DemoDbContext>(options =>
        {
            // options.UseSqlServer(connectionString);
            options.UseSqlite(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        serviceCollection.AddStaticContent().WithMarkdownFormatter()    
                                       .WithDbStaticContentStore<DemoDbContext>();
  
        var serviceProvider = serviceCollection.BuildServiceProvider();


        _dbStaticContentStore = serviceProvider.GetService<IStaticContentStore>();

        var logger = serviceProvider.GetService<ILogger<DbStaticContentStoreFreeSql>>();
        _dbStaticContentStoreFreeSql = new DbStaticContentStoreFreeSql(_fsql, logger);
    }


    [Benchmark(Baseline = true)]
    public async Task DbStaticContentStoreAsync()
    {
        await _dbStaticContentStore.GetSource("welcome");
        await _dbStaticContentStore.GetSource("privacy");
    }

    [Benchmark]
    public async Task DbStaticContentStoreFreeSqlAsync()
    {
        await _dbStaticContentStoreFreeSql.GetSource("welcome");
        await _dbStaticContentStoreFreeSql.GetSource("privacy");
    }


    //[Benchmark]
    //public async Task FreeSqlAsync()
    //{
    //    //await Orm.fsql.Select<StaticContentItem>().ToListAsync();
    //   // await Orm.fsql.Select<StaticContentItem>().Where(p => p.Key == "privacy").ToListAsync();
    //    //await Orm.fsql.Select<StaticContentItem>().Where(p => p.Key == "welcome").ToListAsync();
    //}
}
