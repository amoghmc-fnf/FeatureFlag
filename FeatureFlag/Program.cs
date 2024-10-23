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

            await BasicFunctionality.ConfigureServices(services);
        }
    }

    public static class BasicFunctionality
    {
        public static async Task ConfigureServices(IServiceCollection services)
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IFeatureManager featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

                int[] features = [1, 2, 3];

                string[] stringFeatures = features.Select(x => x.ToString()).ToArray();
                foreach (var FeatureName in stringFeatures)
                {
                    bool enabled = await featureManager.IsEnabledAsync(FeatureName);

                    // Output results
                    Console.WriteLine($"Feature {FeatureName} is {(enabled ? "enabled" : "disabled")}"); 
                }
            }
        }
    }
}
