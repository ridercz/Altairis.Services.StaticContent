using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.Services.StaticContent;
public class DbStaticContentStoreFreeSql : IStaticContentStore
{
    private IFreeSql _freeSql;
    private readonly ILogger<DbStaticContentStoreFreeSql> _logger;

    public DbStaticContentStoreFreeSql(IFreeSql freeSql, ILogger<DbStaticContentStoreFreeSql> logger)
    {
        if (freeSql is null) throw new ArgumentNullException(nameof(freeSql));
        if(logger is null) throw new ArgumentNullException(nameof(logger)); 

        _freeSql = freeSql;
        _logger = logger;
    }

    public async Task<string> GetSource(string key)
    {
        var items = await _freeSql.Select<StaticContentItem>().Where(p => p.Key == key).ToListAsync();
        
        if (!items.Any())
        {           
            _logger.LogError("Static page with key {key} was not found in store.", key);
            
            return string.Empty;
        }
        return items.Single().Text;
    }
}