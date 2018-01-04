using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

using Tc.Psg.Logging;

namespace Microsoft.Extensions.Logging
{
    public static partial class ILoggerFactoryExtensions
    {
        public static ILoggerFactory AddPsgLogging(this ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            PsgLoggingOptions options = configuration.Get<PsgLoggingOptions>();

            return loggerFactory.AddPsgLogging(options);
        }

        public static ILoggerFactory AddPsgLogging(this ILoggerFactory loggerFactory, PsgLoggingOptions options)
        {
            LoggingLevelSwitch loggingLevelSwitch = new LoggingLevelSwitch();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Seq(options.SeqServerUrl, apiKey: options.SeqApiKey, controlLevelSwitch: loggingLevelSwitch)
                .CreateLogger();

            return loggerFactory.AddSerilog();
        }
    }
}
