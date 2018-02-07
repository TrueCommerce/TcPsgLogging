using System;

using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

using Tc.Psg.Logging;


namespace Microsoft.Extensions.Logging
{
    public static partial class ILoggerFactoryExtensions
    {
        /// <summary>
        /// Adds PSG Logging to the logging pipeline.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration">The configuration to use when setting up logging.</param>
        /// <returns></returns>
        public static ILoggerFactory AddPsgLogging(this ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            PsgLoggingOptions options = configuration.Get<PsgLoggingOptions>();

            return loggerFactory.AddPsgLogging(options);
        }

        /// <summary>
        /// Adds PSG Logging to the logging pipeline.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configure">The configuration provider to call when setting up logging.</param>
        /// <returns></returns>
        public static ILoggerFactory AddPsgLogging(this ILoggerFactory loggerFactory, Action<PsgLoggingOptions> configure)
        {
            PsgLoggingOptions options = new PsgLoggingOptions();

            configure?.Invoke(options);

            return loggerFactory.AddPsgLogging(options);
        }

        /// <summary>
        /// Adds PSG Logging to the logging pipeline.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="options">The options to use when setting up logging.</param>
        /// <returns></returns>
        public static ILoggerFactory AddPsgLogging(this ILoggerFactory loggerFactory, PsgLoggingOptions options)
        {
            LogEventLevel initialLogLevel = LogEventLevel.Debug;

            if (!string.IsNullOrWhiteSpace(options.MinimumLogLevel))
            {
                Enum.TryParse(options.MinimumLogLevel, out initialLogLevel);
            }

            LoggingLevelSwitch loggingLevelSwitch = new LoggingLevelSwitch(initialLogLevel);

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

            if (options.UseRollingFiles)
            {
                loggerConfiguration.WriteTo.RollingFile(options.GetFullRollingFileName(),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}  [{Level}]  {SourceContext}  {Message}{NewLine}{Exception}",
                    shared: options.UseMultiProcessLogging
                    );
            }

            if (options.UseSeq)
            {
                loggerConfiguration.WriteTo.Seq(options.SeqServerUrl, apiKey: options.SeqApiKey, controlLevelSwitch: loggingLevelSwitch);
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            return loggerFactory.AddSerilog();
        }
    }
}
