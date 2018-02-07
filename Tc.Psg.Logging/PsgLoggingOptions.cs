using System;
using System.IO;
using System.Reflection;

namespace Tc.Psg.Logging
{
    /// <summary>
    /// Options used to configure PSG Logging.
    /// </summary>
    public class PsgLoggingOptions
    {
        private const string _DefaultStoragePathFragment = @"TrueCommerce\PSGEngineering\Logging";

        public PsgLoggingOptions()
        {
            MinimumLogLevel = "Warning";
            RollingFilePrefix = Assembly.GetEntryAssembly().GetName().Name;
            SeqServerUrl = "http://seq.tcpsg.net:80";
            StoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _DefaultStoragePathFragment);
        }

        /// <summary>
        /// Sets the minimum logging level. This setting does not apply if Seq is being used as the level is controlled by Seq.
        /// </summary>
        public string MinimumLogLevel { get; set; }

        /// <summary>
        /// The prefix that should be applied to the rolling file names.
        /// </summary>
        public string RollingFilePrefix { get; set; }

        /// <summary>
        /// The API Key to use when sending events to the Seq log server.
        /// </summary>
        public string SeqApiKey { get; set; }

        /// <summary>
        /// The URL of the central Seq log server.
        /// </summary>
        public string SeqServerUrl { get; set; }

        /// <summary>
        /// The full path to the directory where log files should be stored.
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// Determines if the logging framework should allow multiple processes to write to the same file.
        /// </summary>
        public bool UseMultiProcessLogging { get; set; }

        /// <summary>
        /// Determines whether or not log events should be written to rolling files in the storage path.
        /// </summary>
        public bool UseRollingFiles { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not log events will be shipped to Seq.
        /// </summary>
        public bool UseSeq
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(SeqApiKey) && !string.IsNullOrWhiteSpace(SeqServerUrl));
            }
        }

        public string GetFullRollingFileName()
        {
            TryEnsurePathsExist();

            return Path.Combine(StoragePath, string.Concat(RollingFilePrefix, "Log-{Date}.txt"));
        }

        public void TryEnsurePathsExist()
        {
            try
            {
                if (!Directory.Exists(StoragePath))
                {
                    Directory.CreateDirectory(StoragePath);
                }
            }

            catch { }
        }
    }
}
