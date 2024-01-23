using StackExchange.Redis;
using System.Collections.Concurrent;

public interface ITenantRedisCacheProvider
{
    IDatabase GetDatabase(string tenantId);
}

public class TenantRedisCacheProvider : ITenantRedisCacheProvider
{
    private readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connectionMultiplexers = new ConcurrentDictionary<string, ConnectionMultiplexer>();
    private readonly IConfiguration _configuration;

    public TenantRedisCacheProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDatabase GetDatabase(string tenantId)
    {
        var connectionMultiplexer = _connectionMultiplexers.GetOrAdd(tenantId, (tenant) =>
        {
            // Here you should implement the logic to retrieve the connection string for the tenant
            // For example, it might come from a configuration file, a database, etc.
            string connectionString = GetConnectionStringForTenant(tenant);
            return ConnectionMultiplexer.Connect(connectionString);
        });

        return connectionMultiplexer.GetDatabase();
    }

    private string GetConnectionStringForTenant(string tenantId)
    {
        // Implement your logic to get the connection string based on the tenantId
        // For example, you might fetch this from a configuration file or a database
        return _configuration[$"RedisConnections:{tenantId}"];
    }
}
