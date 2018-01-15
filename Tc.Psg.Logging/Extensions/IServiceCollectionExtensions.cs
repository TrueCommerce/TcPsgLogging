using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// This method allows Trace and Debug events to be sent to Serilog by the Microsoft logging framework.
        /// The Seq server still manages the minimum Serilog logging level to ensure Debug events aren't broadcast unless we want them.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection EnableTraceLogging(this IServiceCollection services)
        {

            return services.AddSingleton<IConfigureOptions<LoggerFilterOptions>>(new ConfigureOptions<LoggerFilterOptions>(options =>
            {
                options.MinLevel = LogLevel.Trace;
            }));
        }
    }
}
