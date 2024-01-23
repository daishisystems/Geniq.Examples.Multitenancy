public class TenantRedisCacheMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITenantRedisCacheProvider _tenantRedisCacheProvider;

    public TenantRedisCacheMiddleware(RequestDelegate next, ITenantRedisCacheProvider tenantRedisCacheProvider)
    {
        _next = next;
        _tenantRedisCacheProvider = tenantRedisCacheProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Retrieve tenant ID from the cookie
        var tenantId = context.Request.Cookies["TenantId"];

        if (!string.IsNullOrEmpty(tenantId))
        {
            // Get the tenant-specific Redis database
            var database = _tenantRedisCacheProvider.GetDatabase(tenantId);

            // Optionally, you might want to store the database instance in the HttpContext
            // so other parts of your request pipeline can use it
            context.Items["TenantRedisDatabase"] = database;

            // ... Perform actions with the database as needed

        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}
