namespace eShop.Cart.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            try
            {
                var multiplexer = ConnectionMultiplexer.Connect(configuration);
                return multiplexer;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not connect to Redis", ex);
            }
        });
    }
}