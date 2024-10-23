using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace FeatureFlag
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            // Setup application services + feature management
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(configuration)
                    .AddFeatureManagement();

            await Startup.ConfigureServices(services);
        }
    }

    public static class Startup
    {
        public static async Task ConfigureServices(IServiceCollection services)
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IFeatureManager featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

                const string FeatureName = "FeatureU";

                bool enabled = await featureManager.IsEnabledAsync(FeatureName);

                // Output results
                Console.WriteLine($"The {FeatureName} feature is {(enabled ? "enabled" : "disabled")}");
            }
        }
    }
}
